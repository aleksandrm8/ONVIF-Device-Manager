using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace utils {

	public static class XmlExtensions {

		public static Object Deserialize(this XmlNode xmlNode, Type type) {
			var xroot = type.GetCustomAttribute<XmlRootAttribute>();
			if (xroot == null) {
				var xtype = type.GetCustomAttribute<XmlTypeAttribute>();
				if (xtype != null && xmlNode.NodeType == XmlNodeType.Element) {
					return Deserialize(xmlNode, type, new XmlQualifiedName(xmlNode.LocalName, xmlNode.NamespaceURI));
				}
			}

			using (var reader = new XmlNodeReader(xmlNode)) {
				return Deserialize(reader, type);
			}
		}

		public static Object Deserialize(this XmlNode xmlNode, Type type, XmlQualifiedName root) {
			using (var reader = new XmlNodeReader(xmlNode)) {
				return Deserialize(reader, type, root);
			}
		}

		public static T Deserialize<T>(this XmlNode xmlNode) {
			return (T)Deserialize(xmlNode, typeof(T));
		}

		public static T Deserialize<T>(this XmlNode xmlNode, XmlQualifiedName root) {
			using (var reader = new XmlNodeReader(xmlNode)) {
				return Deserialize<T>(reader, root);
			}
		}

		private static Dictionary<Tuple<Type, XmlQualifiedName>, XmlSerializer> serializers = new Dictionary<Tuple<Type, XmlQualifiedName>, XmlSerializer>();

		public static XmlSerializer GetSerializer(Type type, XmlQualifiedName qname = null) {
			var key = new Tuple<Type, XmlQualifiedName>(type, qname);
			XmlSerializer serializer;
			lock (serializers) {
				if (!serializers.TryGetValue(key, out serializer)) {
					if (qname == null || qname.IsEmpty) {
						var xroot = type.GetCustomAttribute<XmlRootAttribute>();
						if (xroot == null) {
							string rootName = type.Name;
							string rootNs = null;
							var xtype = type.GetCustomAttribute<XmlTypeAttribute>();
							if (xtype != null) {
								if (!String.IsNullOrEmpty(xtype.TypeName)) {
									rootName = xtype.TypeName;
								}
								rootNs = xtype.Namespace;
							}
							xroot = new XmlRootAttribute(rootName) { Namespace = rootNs };
						}
						var rkey = new Tuple<Type, XmlQualifiedName>(type, new XmlQualifiedName(xroot.ElementName, xroot.Namespace));
						if (!serializers.TryGetValue(rkey, out serializer)) {
							serializer = new XmlSerializer(type, xroot);
							serializers.Add(rkey, serializer);
						}
					} else {
						serializer = new XmlSerializer(type, new XmlRootAttribute(qname.Name) { Namespace = qname.Namespace });
					}
					serializers.Add(key, serializer);
				}
			}
			return serializer;
		}

		public static Object Deserialize(this XNode xNode, Type type) {
			var serializer = GetSerializer(type, null);
			using (var reader = xNode.CreateReader()) {
				return serializer.Deserialize(reader);
			}
		}

		public static Object Deserialize(this XNode xNode, Type type, XName root) {
			using (var reader = xNode.CreateReader()) {
				return Deserialize(reader, type, root);
			}
		}

		public static T Deserialize<T>(this XNode xNode) {
			return (T)Deserialize(xNode, typeof(T));
		}

		public static T Deserialize<T>(this XNode xNode, XName root) {
			return (T)Deserialize(xNode, typeof(T), root);
		}

		public static Object Deserialize(this XmlReader xmlReader, Type type) {
			var serializer = GetSerializer(type, null);
			return serializer.Deserialize(xmlReader);
		}

		public static Object Deserialize(this XmlReader xmlReader, Type type, string rootName, string rootNamespace) {
			var qname = new XmlQualifiedName(rootName, rootNamespace);
			var serializer = GetSerializer(type, qname);
			return serializer.Deserialize(xmlReader);
		}

		public static Object Deserialize(this XmlReader xmlReader, Type type, XmlQualifiedName root) {
			if (root != null) {
				return Deserialize(xmlReader, type, root.Name, root.Namespace);
			}
			return Deserialize(xmlReader, type);
		}

		public static Object Deserialize(this XmlReader xmlReader, Type type, XName root) {
			if (root != null) {
				return Deserialize(xmlReader, type, root.LocalName, root.NamespaceName);
			}
			return Deserialize(xmlReader, type);
		}

		public static T Deserialize<T>(this XmlReader xmlReader) {
			return (T)Deserialize(xmlReader, typeof(T));
		}

		public static T Deserialize<T>(this XmlReader xmlReader, XmlQualifiedName root) {
			return (T)Deserialize(xmlReader, typeof(T), root);
		}

		public static T Deserialize<T>(this XmlReader xmlReader, XName root) {
			return (T)Deserialize(xmlReader, typeof(T), root.LocalName, root.NamespaceName);
		}

		public static XmlElement Serialize(this object obj) {
			var serializer = GetSerializer(obj.GetType());
			var xmlDoc = new XmlDocument();
			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj);
			}
			return xmlDoc.DocumentElement;
		}

		public static XmlElement Serialize<T>(this T obj) {
			var ns = new XmlSerializerNamespaces();
			var xroot = typeof(T).GetCustomAttribute<XmlRootAttribute>();
			if (xroot != null) {
				ns.Add("", xroot.Namespace);
			} else {
				ns.Add("", "");
			}
			return obj.Serialize(ns);
		}

		public static XElement SerializeAsXElement<T>(this T obj) {
			var ns = new XmlSerializerNamespaces();
			var xroot = typeof(T).GetCustomAttribute<XmlRootAttribute>();
			if (xroot != null) {
				ns.Add("", xroot.Namespace);
			} else {
				ns.Add("", "");
			}
			return obj.SerializeAsXElement(ns);
		}

		public static XElement SerializeAsXElement<T>(this T obj, XmlSerializerNamespaces ns) {
			var serializer = GetSerializer(typeof(T));
			var xd = new XDocument();
			using (var writer = xd.CreateWriter()) {
				serializer.Serialize(writer, obj, ns);
			}
			var root = xd.Root;
			root.Remove();
			return root;
		}

		public static XmlElement Serialize<T>(this T obj, XmlSerializerNamespaces ns) {
			var serializer = GetSerializer(typeof(T));
			var xmlDoc = new XmlDocument();
			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj, ns);
			}
			return xmlDoc.DocumentElement;
		}

		public static XmlElement Serialize<T>(this T obj, XmlQualifiedName root) {
			var serializer = GetSerializer(typeof(T), root);
			var xmlDoc = new XmlDocument();

			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj);
			}

			return xmlDoc.DocumentElement;
		}

		public static XmlElement ToXmlElement(this XElement xelement) {
			var xmlDoc = new XmlDocument();
			using (var reader = xelement.CreateReader()) {
				xmlDoc.Load(reader);
			}
			return xmlDoc.DocumentElement;
		}
		public static XName ToXName(this XmlQualifiedName qname) {
			return XName.Get(qname.Name, qname.Namespace);
		}

		public static IDictionary<string, string> GetNamespacesInScope(this XmlReader reader) {
			if (reader == null) {
				return null;
			}
			var xmlResolver = reader as IXmlNamespaceResolver;
			if (xmlResolver != null) {
				return xmlResolver.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);
			}
			var asmSysRuntimeSerialization = Assembly.Load("System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			var t = asmSysRuntimeSerialization.GetType("System.Xml.XmlBaseReader");
			var readerType = reader.GetType();
			if (t.IsAssignableFrom(readerType)) {
				var nsMgr = ExposedObject.From(ExposedObject.From(reader).nsMgr);
				var namespaces = nsMgr.namespaces as object[];
				if (namespaces == null) {
					return null;
				}
				var dic = new Dictionary<string, string>();
				for (int i = nsMgr.nsCount - 1; i >= 0; i--) {
					var ns = ExposedObject.From(namespaces[i]);
					if (ns == null) {
						continue;
					}
					var prefix = ExposedObject.From(ns.Prefix);
					if (prefix == null || prefix.IsEmpty || prefix.IsXml || prefix.IsXmlns) {
						continue;
					}
					var key = prefix.GetString() as string;
					if (String.IsNullOrEmpty(key)) {
						continue;
					}
					var uri = ExposedObject.From(ns.Uri);
					string value = uri != null ? uri.GetString() : null;
					dic[key] = value ?? String.Empty;
				}
				return dic;
			}
			return null;
		}

		public static bool Any(this XmlAttributeCollection attrs, Func<XmlAttribute, bool> pred) {
			return attrs.OfType<XmlAttribute>().Any(pred);
		}

		public static void ForEachElement(this XmlNodeList nodes, Action<XmlElement> action) {
			foreach (var node in nodes.OfType<XmlElement>()) {
				action(node);
			}
		}

		//TODO: very specific functional, needs to be scoped
		public static object ConvertXSValue(XmlQualifiedName type, string value) {
			const string ns = @"http://www.w3.org/2001/XMLSchema";
			if (type == new XmlQualifiedName("boolean", ns))
				return value == null ? false : XmlConvert.ToBoolean(value.ToLowerInvariant()); //value.ParseBoolInvariant();
			else if (type == new XmlQualifiedName("integer", ns))
				return value == null ? 0 : XmlConvert.ToInt32(value);//int.Parse(value);
			//skip double for good
			return value ?? string.Empty;
		}

		public static string ConvertBackXSValue(object value) {
			if (value == null) {
				return null;
			}

			var _string = value as string;
			if (_string != null) {
				return _string;
			}

			var _double = value as double?;
			if (_double.HasValue) {
				return XmlConvert.ToString(_double.Value);
			}

			var _float = value as float?;
			if (_float.HasValue) {
				return XmlConvert.ToString(_float.Value);
			}

			var _int = value as int?;
			if (_int.HasValue) {
				return XmlConvert.ToString(_int.Value);
			}
			
			var _bool = value as bool?;
			if (_bool.HasValue) {
				return XmlConvert.ToString(_bool.Value);
			}
			return value.ToString();
		}
	}
}
