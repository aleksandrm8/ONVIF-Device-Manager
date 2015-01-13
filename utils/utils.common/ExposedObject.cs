using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Reflection;

namespace utils {
	public class ExposedObject : DynamicObject {


		protected class ExposedObjectHelper {
			private static Type s_csharpInvokePropertyType =
				typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
					.Assembly
					.GetType("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder");

			public static bool InvokeBestMethod(object[] args, object target, List<MethodInfo> instanceMethods, out object result) {
				if (instanceMethods.Count == 1) {
					// Just one matching instance method - call it
					if (TryInvoke(instanceMethods[0], target, args, out result)) {
						return true;
					}
				} else if (instanceMethods.Count > 1) {
					// Find a method with best matching parameters
					MethodInfo best = null;
					Type[] bestParams = null;
					Type[] actualParams = args.Select(p => p == null ? typeof(object) : p.GetType()).ToArray();

					Func<Type[], Type[], bool> isAssignableFrom = (a, b) => {
						for (int i = 0; i < a.Length; i++) {
							if (!a[i].IsAssignableFrom(b[i])) return false;
						}
						return true;
					};


					foreach (var method in instanceMethods.Where(m => m.GetParameters().Length == args.Length)) {
						Type[] mParams = method.GetParameters().Select(x => x.ParameterType).ToArray();
						if (isAssignableFrom(mParams, actualParams)) {
							if (best == null || isAssignableFrom(bestParams, mParams)) {
								best = method;
								bestParams = mParams;
							}
						}
					}

					if (best != null && TryInvoke(best, target, args, out result)) {
						return true;
					}
				}

				result = null;
				return false;
			}

			public static bool TryInvoke(MethodInfo methodInfo, object target, object[] args, out object result) {
				try {
					result = methodInfo.Invoke(target, args);
					return true;
				} catch (TargetInvocationException) {
				} catch (TargetParameterCountException) {
				}

				result = null;
				return false;

			}

			public static Type[] GetTypeArgs(InvokeMemberBinder binder) {
				if (s_csharpInvokePropertyType.IsInstanceOfType(binder)) {
					PropertyInfo typeArgsProperty = s_csharpInvokePropertyType.GetProperty("TypeArguments");
					return ((IEnumerable<Type>)typeArgsProperty.GetValue(binder, null)).ToArray();
				}
				return null;
			}

		}

		private object m_object;
		private Type m_type;
		private Dictionary<string, Dictionary<int, List<MethodInfo>>> m_instanceMethods;
		private Dictionary<string, Dictionary<int, List<MethodInfo>>> m_genInstanceMethods;

		private ExposedObject(object obj) {
			m_object = obj;
			m_type = obj.GetType();

			m_instanceMethods =
				m_type
					.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
					.Where(m => !m.IsGenericMethod)
					.GroupBy(m => m.Name)
					.ToDictionary(
						p => p.Key,
						p => p.GroupBy(r => r.GetParameters().Length).ToDictionary(r => r.Key, r => r.ToList()));

			m_genInstanceMethods =
				m_type
					.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
					.Where(m => m.IsGenericMethod)
					.GroupBy(m => m.Name)
					.ToDictionary(
						p => p.Key,
						p => p.GroupBy(r => r.GetParameters().Length).ToDictionary(r => r.Key, r => r.ToList()));


		}

		public object Object { get { return m_object; } }

		public static dynamic New<T>() {
			return New(typeof(T));
		}

		public static dynamic New(Type type) {
			return From(Activator.CreateInstance(type));
		}

		public static dynamic From(object obj) {
			if (obj == null) {
				return null;
			}
			return new ExposedObject(obj);
		}

		public static T Cast<T>(ExposedObject t) {
			return (T)t.m_object;
		}

		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
			// Get type args of the call
			Type[] typeArgs = ExposedObjectHelper.GetTypeArgs(binder);
			if (typeArgs != null && typeArgs.Length == 0) typeArgs = null;

			//
			// Try to call a non-generic instance method
			//
			if (typeArgs == null
					&& m_instanceMethods.ContainsKey(binder.Name)
					&& m_instanceMethods[binder.Name].ContainsKey(args.Length)
					&& ExposedObjectHelper.InvokeBestMethod(args, m_object, m_instanceMethods[binder.Name][args.Length], out result)) {
				return true;
			}

			//
			// Try to call a generic instance method
			//
			if (m_instanceMethods.ContainsKey(binder.Name)
					&& m_instanceMethods[binder.Name].ContainsKey(args.Length)) {
				List<MethodInfo> methods = new List<MethodInfo>();

				foreach (var method in m_genInstanceMethods[binder.Name][args.Length]) {
					if (method.GetGenericArguments().Length == typeArgs.Length) {
						methods.Add(method.MakeGenericMethod(typeArgs));
					}
				}

				if (ExposedObjectHelper.InvokeBestMethod(args, m_object, methods, out result)) {
					return true;
				}
			}

			result = null;
			return false;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value) {
			var propertyInfo = m_type.GetProperty(
				binder.Name,
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			if (propertyInfo != null) {
				propertyInfo.SetValue(m_object, value, null);
				return true;
			}

			var fieldInfo = m_type.GetField(
				binder.Name,
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			if (fieldInfo != null) {
				fieldInfo.SetValue(m_object, value);
				return true;
			}

			return false;
		}

		private bool TryGetMember(Type type, string member, out object result) {
			var propertyInfo = type.GetProperty(member, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			if (propertyInfo != null) {
				result = propertyInfo.GetValue(m_object, null);
				return true;
			}

			var fieldInfo = type.GetField(member, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			if (fieldInfo != null) {
				result = fieldInfo.GetValue(m_object);
				return true;
			}

			if (type.BaseType != null) {
				return TryGetMember(type.BaseType, member, out result);
			}
			result = null;
			return false;
		}
		public override bool TryGetMember(GetMemberBinder binder, out object result) {
			return TryGetMember(m_object.GetType(), binder.Name, out result);
		}

		public override bool TryConvert(ConvertBinder binder, out object result) {
			result = m_object;
			return true;
		}
	}
}
