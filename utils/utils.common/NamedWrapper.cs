using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public class NamedValue {
		public static NamedValue<T> Create<T>(T value, string name) {
			return new NamedValue<T>(value, name);
		}
	}

	public class NamedValue<T> {
		public string name { get; private set; }
		public T value { get; private set; }

		public NamedValue(T value, string name) {
			this.value = value;
			this.name = name;
		}

		public override string ToString() {
			return name;
		}

		public override bool Equals(object obj) {
			if (Object.ReferenceEquals(obj, this)) {
				return true;
			}
			var other = obj as NamedValue<T>;
			if (Object.ReferenceEquals(other, null)) {
				return false;
			}
			return Object.Equals(value, other.value);
		}

		public override int GetHashCode() {
			return value==null ? 0 : value.GetHashCode();
		}

		public static implicit operator T(NamedValue<T> namedValue) {
			return namedValue.value;
		}

	}
}
