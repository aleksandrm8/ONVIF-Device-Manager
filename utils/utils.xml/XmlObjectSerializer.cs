using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Reflection;

namespace utils {
	public static class XmlObjectSerializer {
		private static readonly Type[] WriteTypes = new[] {
			typeof(string), typeof(DateTime), typeof(Enum), 
			typeof(decimal), typeof(Guid),
		};
		public static bool IsSimpleType(this Type type) {
			return type.IsPrimitive || WriteTypes.Contains(type);
		}

		public static XElement ToXml(this object obj, string root = null) {
			if (obj == null) {
				return null;
			}

			if (string.IsNullOrEmpty(root)) {
				root = "object";
			}
			
			var type = obj.GetType();
			var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var xe = new XElement(XmlConvert.EncodeName(root));
			
			props.ForEach(p => {
				var val = p.GetValue(obj, null);
				if (val != null) {
					var name = p.Name;
					if (p.PropertyType.IsSimpleType()) {
						xe.Add(new XElement(name, val));
					} else {
						xe.Add(val.ToXml(name));
					}
				}
			});

			return xe;
		}
	}
}
