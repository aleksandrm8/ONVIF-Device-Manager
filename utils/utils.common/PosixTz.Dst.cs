using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	
	public partial class PosixTz {

		public class Dst {
			public readonly string name;
			//Indicates the number of seconds added to the local time to arrive at Coordinated Universal Time.
			public readonly int offset;
			public readonly DstRule start;
			public readonly DstRule end;
			public Dst(string name, int offset, DstRule start, DstRule end) {
				this.name = name;
				this.offset = offset;
				this.start = start;
				this.end = end;
			}
			public static Dst UnnamedForm(Dst dst) {
				if ((object)dst == null) {
					return null;
				}
				return new Dst(null, dst.offset, dst.start, dst.end);
			}
			public static bool LogicallyEquals(Dst left, Dst right) {
				return Object.ReferenceEquals(left, right) || (
					!Object.ReferenceEquals(left, null) &&
					!Object.ReferenceEquals(right, null) &&
					left.offset == right.offset && 
					left.start == right.start &&
					left.end == right.end
				);
			}
			public bool LogicallyEquals(Dst other) {
				return LogicallyEquals(this, other);
			}
			public static int GetLogicalHashCode(Dst dst) {
				if ((object)dst == null) {
					return 0;
				}
				return HashCode.Combine(
					dst.offset, dst.start.GetHashCode(), dst.end.GetHashCode()
				);
			}
			public int GetLogicalHashCode() {
				return GetLogicalHashCode(this);
			}
			public override bool Equals(object obj) {
				return Equals(obj as Dst);
			}
			public bool Equals(Dst other) {
				return
					!Object.ReferenceEquals(other, null) &&
					offset == other.offset && 
					start == other.start &&
					end == other.end &&
					name == other.name;
			}
			public override int GetHashCode() {
				return HashCode.Combine(
					offset, start.GetHashCode(), end.GetHashCode(), HashCode.Get(name)
				);
			}
			public static bool operator ==(Dst left, Dst right) {
				if (Object.ReferenceEquals(left, right)) {
					return true;
				}
				return !Object.ReferenceEquals(left, null) && left.Equals(right);
			}
			public static bool operator !=(Dst left, Dst right) {
				return !(left == right);
			}
		}
		
	}
}
