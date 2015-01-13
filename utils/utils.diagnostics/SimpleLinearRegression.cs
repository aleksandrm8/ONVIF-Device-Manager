using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {

	/// <summary>
	/// Linear regression model estmated by ordinary least squares
	/// </summary>
	public class SimpleLinearRegression {
		double step = 1.0;
		double Sx = 0;
		double Sy = 0;
		double Sxy = 0;
		double Sxx = 0;
		int n = 0;

		/// <summary>
		/// add new point
		/// </summary>
		/// <param name="y">output of function over input x</param>
		/// <param name="x">input over which y was evaluated</param>
		public void Next(double y, double x) {
			Sx += x;
			Sy += y;
			Sxy += x * y;
			Sxx += x * x;
			++n;
		}

		/// <summary>
		/// add new point
		/// </summary>
		/// <param name="y">output of function over input n*step, where n - count of added points</param>
		public void Next(double y) {
			Next(y, step*n);
		}

		/// <summary>
		/// return pair (a,b) parameters of linear function l(x) = a*x + b
		/// </summary>
		/// <returns></returns>
		public Tuple<double, double> GetParameters() {
			if (n < 2) {
				return Tuple.Create(0.0, 0.0);
			}
			double dn = n;
			double a = (Sxy * dn - Sx * Sy) / (Sxx * dn - Sx * Sx);
			double b = (Sy - a * Sx) / dn;
			return Tuple.Create(a, b);
		}
	}

}
