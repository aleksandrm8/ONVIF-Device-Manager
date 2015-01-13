using System;
using System.Collections;
using System.Collections.Generic;

namespace utils {
	public class CircularBuffer<T> : IEnumerable<T> {
		protected T[] innerBuffer = null;
		protected int head = 0;
		
		public int length {get; private set;}

		public T first {
			get {
				return this[0];
			}
		}

		public T last {
			get {
				return this[length - 1];
			}
		}

		public T this[int index] {
			get {
				if (index >= length || index < 0) {
					throw new ArgumentOutOfRangeException("index");
				}
				return innerBuffer[GetItemPosition(index)];
			}
		}

		public CircularBuffer(int size) {
			if (size <= 0) {
				throw new ArgumentOutOfRangeException("size");
			}
			innerBuffer = new T[size];
		}

		public void Enqueue(T value) {
			innerBuffer[GetItemPosition(length)] = value;
			if (length < capacity) {
				++length;
			} else {
				head = (head + 1) % capacity;
			}
		}
		
		public bool TryDequeue(out T value) {
			if (length == 0) {
				value = default(T);
				return false;
			}
			length = length-1;
			var pos = GetItemPosition(0);
			value = innerBuffer[pos];
			innerBuffer[pos] = default(T);
			if (length == 0) {
				head = 0;
			} else {
				head = (head + capacity-1) % capacity;
			}
			return true;
		}

		public T DequeueOrDefault() {
			T value;
			TryDequeue(out value);
			return value;
		}

		public void Clear() {
			length = 0;
			head = 0;
		}

		public int capacity {
			get {
				return innerBuffer.Length;
			}
		}

		public IEnumerator<T> GetEnumerator() {
			for (int i = 0; i < length; ++i) {
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			for (int i = 0; i < length; ++i) {
				yield return this[i];
			}
		}

		private int GetItemPosition(int index) {
			return (head + index) % capacity;
		}
	}

}
