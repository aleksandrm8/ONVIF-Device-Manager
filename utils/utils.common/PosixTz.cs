using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {

	public partial class PosixTz: IEquatable<PosixTz> {
		public const int TZNAME_MAX = 255;
		public const string DEFAULT_STD_NAME = "STD";
		public const string DEFAULT_DST_NAME = "DST";
		public static readonly TimeUnit DEFAULT_RULE_TIME = new TimeUnit(2, 0, 0);

		public readonly string name;
		//Indicates the number of seconds added to the local time to arrive at Coordinated Universal Time.
		public readonly int offset;
		public readonly Dst dst;
		
		public PosixTz(string name, int offset, Dst dst) {
			//if (dst != null && dst.offset > offset) {
			//    var new_dst = new Dst(
			//        name, offset, dst.end, dst.start
			//    );
			//    name = dst.name;
			//    offset = dst.offset;
			//    dst = new_dst;
			//}
			this.name = name;
			this.offset = offset;
			this.dst = dst;
		}

		public static int GetDefaultDstOffset(int stdOffsetInSeconds) {
			//if no offset follows dst, the alternative time is assumed to be one hour ahead 
			//of standard time.
			return stdOffsetInSeconds - TAI.SecondsPerHour;
		}

		public string Format() {
			var writer = new PosixTzWriter();
			writer.WriteTimeZone(this);
			return writer.ToString();
		}

		public static PosixTz Parse(string str) {
			var reader = new PosixTzReader(str);
			return reader.ReadExpandedTimeZone();
		}

		public static PosixTz TryParse(string str) {
			try {
				var reader = new PosixTzReader(str);
				return reader.ReadExpandedTimeZone();
			} catch {
				return null;
			}
		}

		static PosixTz UnnamedForm(PosixTz posixTz) {
			if ((object)posixTz == null) {
				return null;
			}
			return new PosixTz(null, posixTz.offset, Dst.UnnamedForm(posixTz.dst));
		}
		//public static PosixTz InvertedForm(PosixTz posixTz) {
		//    if ((object)posixTz == null || (object)posixTz.dst == null) {
		//        return null;
		//    }
		//    return new PosixTz(
		//        posixTz.dst.name, posixTz.dst.offset, 
		//        new PosixTz.Dst(
		//            posixTz.name, posixTz.offset,
		//            posixTz.dst.end, posixTz.dst.start
		//        )
		//    );
		//}

		public static bool LogicallyEquals(PosixTz left, PosixTz right) {
			return Object.ReferenceEquals(left, right) || (
				!Object.ReferenceEquals(left, null) && !Object.ReferenceEquals(right, null) &&
				left.offset == right.offset && Dst.LogicallyEquals(left.dst, right.dst)
			);
		}
		public bool LogicallyEquals(PosixTz other) {
			return LogicallyEquals(this, other);
		}
		public static int GetLogicalHashCode(PosixTz posixTz) {
			if ((object)posixTz == null) {
				return 0;
			}
			return HashCode.Combine(
				posixTz.offset, Dst.GetLogicalHashCode(posixTz.dst)
			);
		}
		public int GetLogicalHashCode() {
			return GetLogicalHashCode(this);
		}

		public override bool Equals(object obj) {
			return this == (obj as PosixTz);
		}
		public bool Equals(PosixTz other) {
			return this == other;
		}
		public override int GetHashCode() {
			return HashCode.Combine(
				offset, HashCode.Get(dst), HashCode.Get(name)
			);
		}
		public static bool operator == (PosixTz left, PosixTz right) {
			return Object.ReferenceEquals(left, right) || (
				!Object.ReferenceEquals(left, null) && 
				!Object.ReferenceEquals(right, null) &&
				left.name == right.name &&
				left.offset == right.offset && 
				left.dst == right.dst
			);
		}
		public static bool operator != (PosixTz left, PosixTz right) {
			return !(left == right);
		}
	}
}
