using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace utils {
	public static class Async {
		public static async Task<T> TryCatch<T>(Func<Task<T>> @try, Func<Exception, Task<T>> @catch) {
			Task<T> econt;
			try {
				return await @try();
			} catch (Exception err) {
				econt = @catch(err);
			}
			return await econt;
		}
		public static async Task TryCatch(Func<Task> @try, Func<Exception, Task> @catch) {
			Task econt;
			try {
				await @try();
				return;
			} catch (Exception err) {
				econt = @catch(err);
			}
			await econt;
		}
		public static async Task<T> Invoke<T>(Func<Task<T>> func) {
			return await func();
		}
	}
}
