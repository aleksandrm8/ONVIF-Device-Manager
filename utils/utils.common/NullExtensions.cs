using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace utils {
	public static class NullExtensions {
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static TRes Map<TVal, TRes>(this TVal val, Func<TVal, TRes> transformation) {
			return transformation(val);
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool NullTest<T>(this T val, Action<T> notNull, Action isNull) where T : class {
			if (!object.ReferenceEquals(val, null)) {
				notNull(val);
				return false;
			} else {
				isNull();
				return true;
			}
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static TRes NullTest<TVal, TRes>(this TVal val, Func<TVal, TRes> notNull, Func<TRes> isNull) where TVal : class {
			if (!object.ReferenceEquals(val, null)) {
				return notNull(val);
			} else {
				return isNull();
			}
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IfNull<T>(this T val, Action handler) where T : class {
			var isNull = object.ReferenceEquals(val, null);
			if (isNull) {
				handler();
			}
			return isNull;
		}

		/// <summary>
		/// function equivalent of ??
		/// </summary>
		/// <typeparam name="TVal">value type</typeparam>
		/// <param name="val">value tested for null</param>
		/// <param name="handler">function to be invoked to substitute null values</param>
		/// <returns>value if it's not null or result of handler() otherwise</returns>
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static TVal IfNull<TVal>(this TVal val, Func<TVal> handler) where TVal : class {
			if (object.ReferenceEquals(val, null)) {
				return handler();
			}
			return val;
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IfNotNull<T>(this T val, Action<T> handler) where T : class {
			var notNull = !object.ReferenceEquals(val, null);
			if (notNull) {
				handler(val);
			}
			return notNull;
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static TRes IfNotNull<TVal, TRes>(this TVal val, Func<TVal, TRes> handler) where TVal : class {
			if (!object.ReferenceEquals(val, null)) {
				return handler(val);
			}
			return default(TRes);
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static T EnsureNotNull<T>(this T val, Exception err) where T : class {
			if (!object.ReferenceEquals(val, null)) {
				return val;
			}
			throw err;
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static T EnsureNotNull<T>(this T val, string errMessage) where T : class {
			if (!object.ReferenceEquals(val, null)) {
				return val;
			}
			throw new Exception(errMessage);
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static T EnsureThat<T>(this T val, Func<T, bool> predicate, string errMessage) {
			if (!predicate(val)) {
				throw new Exception(errMessage);

			}
			return val;
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool NullOrEmptyTest(this string val, Action<string> @not, Action @is) {
			if (!String.IsNullOrEmpty(val)) {
				@not(val);
				return false;
			} else {
				@is();
				return true;
			}
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static T NullOrEmptyTest<T>(this string val, Func<string, T> @not, Func<T> @is) {
			if (!String.IsNullOrEmpty(val)) {
				return @not(val);
			} else {
				return @is();
			}
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IsNullOrEmpty(this string src) {
			return String.IsNullOrEmpty(src);
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string IfNullOrEmpty(this string src, string val) {
			if (!src.IsNullOrEmpty()) {
				return src;
			} else {
				return val;
			}
		}

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string IfNullOrEmpty<T>(this string src, T val) {
			if (!src.IsNullOrEmpty()) {
				return src;
			} else {
				return val.ToString();
			}
		}

		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> collection) {
			return collection ?? new T[0];
		}
	}

}
