using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public class HashSetExt<T> : HashSet<T> {
		public bool this[T key] {
			get {
				return Contains(key);
			}
			set {
				if (value) {
					Add(key);
				} else {
					Remove(key);
				}
			}
		}
	}
}
