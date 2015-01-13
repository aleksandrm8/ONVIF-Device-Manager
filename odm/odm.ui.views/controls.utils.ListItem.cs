using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using utils;

namespace odm.ui.controls {
	public class ListItem {
		public static ListItem<T> Wrap<T>(T obj, Func<T, string> display) where T : class {
			return new ListItem<T>(obj, display);
		}
	}
	public class ListItem<T>{
		public ListItem(T obj, Func<T, string> display) {
			m_object = obj;
			m_display = display;
		}
		T m_object;
		Func<T, string> m_display = null;
		public override string ToString() {
			if (m_display != null) {
				return m_display(m_object);
			}
			return m_object.ToString();
		}
		public override bool Equals(object obj) {
			if (m_object == null && obj == null) {
				return true;
			}
			if (obj == null || obj.GetType() != typeof(ListItem<T>)) {
				return false;
			}
			var result = m_object.Equals((obj as ListItem<T>).m_object);
			return result;
		}
		public override int GetHashCode() {
			if (m_object == null) {
				return 0;
			}
			return m_object.GetHashCode();
		}
		public T Unwrap() {
			return m_object;
		}
	}
}
