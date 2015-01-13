using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public static class Lambda {
		public static void Invoke(Action act) {
			act();
		}
		public static TRes Invoke<TRes>(Func<TRes> func) {
			return func();
		}
		public static TRes Invoke<TArg, TRes>(Func<TArg, TRes> func, TArg arg) {
			return func(arg);
		}
		public static TRes Invoke<TArg1, TArg2, TRes>(Func<TArg1, TArg2, TRes> func, TArg1 arg1, TArg2 arg2) {
			return func(arg1, arg2);
		}
		public static TRes Invoke<TArg1, TArg2, TArg3, TRes>(Func<TArg1, TArg2, TArg3, TRes> func, TArg1 arg1, TArg2 arg2, TArg3 arg3) {
			return func(arg1, arg2, arg3);
		}
		public static T TryCatch<T>(Func<T> @try, Func<Exception, T> @catch) {
			try {
				return @try();
			} catch (Exception err) {
				return @catch(err);
			}
		}
	}
}
