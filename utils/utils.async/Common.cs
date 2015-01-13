using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace utils {

	public interface IAwaiter : INotifyCompletion {
		bool IsCompleted { get; }
		void GetResult();
	}
	public interface IAwaiter<TResult> : INotifyCompletion {
		bool IsCompleted { get; }
		TResult GetResult();
	}
	public interface IAwaitable {
		IAwaiter GetAwaiter();
	}
	public interface IAwaitable<TResult> {
		IAwaiter<TResult> GetAwaiter();
	}
	
}
