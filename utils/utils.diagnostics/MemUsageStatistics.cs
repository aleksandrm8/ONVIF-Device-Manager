using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public class MemUsageStatistics {
		public readonly double la;
		public readonly double lb;
		public readonly long min;
		public readonly long max;

		public MemUsageStatistics(double a, double b, long min, long max) {
			this.la = a;
			this.lb = b;
			this.min = min;
			this.max = max;
		}
		public MemUsageStatistics(Tuple<double, double> ab, long min, long max)
			: this(ab.Item1, ab.Item2, min, max) {
		}

		public static MemUsageStatistics Collect(Action action, int iterations) {
			return Collect(_ => {
				action();
				GC.WaitForPendingFinalizers();
				return GC.GetTotalMemory(true);
			}, iterations);
		}

		private class Clojure {
			Action action;
			public Clojure(Action action) {
				this.action = action;
			}
			public static Clojure Create(Action action){
				return new Clojure(action);
			}
			public long MemUsage(int i) {
				action();
				GC.WaitForPendingFinalizers();
				return GC.GetTotalMemory(true);
			}
		}
		public static bool Validate(Action action, int iterations) {
			return Validate(Clojure.Create(action).MemUsage, iterations);
		}

		public static bool Validate(Func<int, long> action, int iterations) {
			var stat = Collect(action, iterations);
			var delta = stat.max - stat.min;
			if (delta > 10 * iterations || Math.Abs(stat.la) > 0.5) {
				return false;
			}
			double lmin;
			double lmax;
			if(stat.la < 0){
				lmin = stat.la * iterations + stat.lb;
				lmax = stat.lb;
			}else{
				lmin = stat.lb;
				lmax = stat.la * iterations + stat.lb;
			}
			var res = lmin >= (stat.min - 0.25 * delta) && lmax <= (stat.max + 0.25 * delta);
			if (!res) {
				return false;
			}
			return true;
		}

		public static MemUsageStatistics Collect(Func<int, long> func, int iterations) {
			if (iterations < 1) {
				return null;
			}
			var slr = new SimpleLinearRegression();

			var memUsg = func(0);
			slr.Next(memUsg, 0);

			var min = memUsg;
			var max = memUsg;

			for (int i = 1; i < iterations; ++i) {
				memUsg = func(i);
				
				slr.Next(memUsg, i);
				if (memUsg > max) {
					max = memUsg;
				} else if (memUsg < min) {
					min = memUsg;
				}
			}

			return new MemUsageStatistics(
				slr.GetParameters(), min, max
			);
		}

		public override string ToString() {
			return String.Format("velocity = {0:F6}x {2}{1:F0}", la, lb, lb < 0 ? "" : "+");
		}
	}

}
