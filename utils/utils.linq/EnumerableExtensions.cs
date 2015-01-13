using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace utils {

	public static class EnumerableExtensions {

		private static IEnumerable<T> SelectImpl<T>(IEnumerable src, Func<Object, T> transform) {
			foreach (var x in src) {
				yield return transform(x);
			}
		}

		public static IEnumerable<T> Select<T>(this IEnumerable src, Func<Object, T> transform) {
			if (src == null) {
				return null;
			}
			return SelectImpl(src, transform);
		}

		private static IEnumerable WhereImpl(this IEnumerable src, Func<Object, bool> predicate) {
			foreach (var x in src) {
				if (predicate(x)) {
					yield return x;
				}
			}
		}

		//public static IEnumerable Where(this IEnumerable src, Func<Object, bool> predicate) {
		//	if (src == null) {
		//		return null;
		//	}
		//	return WhereImpl(src, predicate);
		//}

		public static T FirstOrDefault<T>(this IEnumerable<T> src, Func<T> factory) {
			if (src == null) {
				return factory();
			}

			using (var itor = src.GetEnumerator()) {
				if (itor.MoveNext()) {
					return itor.Current;
				}
			}
			return factory();
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> src, Func<T, bool> predicate, Func<T> factory) {
			if (src == null) {
				return factory();
			}
			using (var itor = src.Where(predicate).GetEnumerator()) {
				if (itor.MoveNext()) {
					return itor.Current;
				}
			}
			return factory();
		}

		public static void ForEach<T>(this IEnumerable<T> src, Action<T> action) {
			if (src == null) {
				return;
			}
			if (action == null) {
				action = (t) => { };
			}
			foreach (var element in src) {
				action(element);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> src, Action<T, int> action) {
			if (src == null) {
				return;
			}
			if (action == null) {
				action = (t, i) => { };
			}
			int index = 0;
			foreach (var element in src) {
				action(element, index);
				++index;
			}
		}

		public static void ForEach(this IEnumerable src, Action<object> action) {
			if (src == null) {
				return;
			}
			if (action == null) {
				action = (o) => { };
			}
			foreach (var element in src) {
				action(element);
			}
		}

		public static void ForEach(this IEnumerable src, Action<object, int> action) {
			if (src == null) {
				return;
			}
			if (action == null) {
				action = (o, i) => { };
			}
			int index = 0;
			foreach (var element in src) {
				action(element, index);
			}
		}

		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> src, T head) {
			if (src == null) {
				return Enumerable.Repeat(head, 1);
			} else {
				return Enumerable.Repeat(head, 1).Concat(src);
			}
		}

		public static IEnumerable<T> Append<T>(this IEnumerable<T> src, T tail) {
			if (src == null) {
				return Enumerable.Repeat(tail, 1);
			} else {
				return src.Concat(Enumerable.Repeat(tail, 1));
			}
		}

		/// <summary>
		/// excludes all occurrences of item in src, based on equality
		/// </summary>
		/// <typeparam name="T">type of elements of sequnce</typeparam>
		/// <param name="src">input sequnce</param>
		/// <param name="item">element to exclude</param>
		/// <returns>output sequence</returns>
		public static IEnumerable<T> RemoveAll<T>(this IEnumerable<T> src, T item) where T : class {
			if (src == null) {
				return null;
			} else {
				return src.Where<T>(x => !Object.Equals(x,item));
			}
		}

		private static IEnumerable<T> RemoveFirstImpl<T>(IEnumerable<T> src, T item) where T : class {
			using (var itor = src.GetEnumerator()) {
				while (true){
					if(!itor.MoveNext()){
						yield break;
					}
					if (!Object.Equals(itor.Current, item)) {
						break;
					}
					yield return itor.Current;
				}
				while (itor.MoveNext()) {
					yield return itor.Current;
				}
			}
		}

		/// <summary>
		/// excludes first occurrence of item in src, based on equality
		/// </summary>
		/// <typeparam name="T">type of elements of sequnce</typeparam>
		/// <param name="src">input sequnce</param>
		/// <param name="item">element to exclude</param>
		/// <returns>output sequence</returns>
		public static IEnumerable<T> RemoveFirst<T>(this IEnumerable<T> src, T item) where T : class {
			if (src == null) {
				return null;
			} else {
				return RemoveFirstImpl(src, item);
			}
		}

		public static IEnumerable<T> Repeat<T>(this IEnumerable<T> src, int times) {
			if (src == null) {
				yield break;
			}
			for (int i = 0; i < times; ++i) {
				foreach (var x in src) {
					yield return x;
				}
			}
		}

		/// <summary>
		/// Returns the maximal element of the given sequence, based on
		/// the given projection and the specified comparer for projected values. 
		/// </summary>
		/// <remarks>
		/// If more than one element has the maximal projected value, the first
		/// one encountered will be returned. This overload uses the default comparer
		/// for the projected type. This operator uses immediate execution, but
		/// only buffers a single result (the current maximal element).
		/// </remarks>
		/// <typeparam name="TSource">Type of the source sequence</typeparam>
		/// <typeparam name="TKey">Type of the projected element</typeparam>
		/// <param name="source">Source sequence</param>
		/// <param name="selector">Selector to use to pick the results to compare</param>
		/// <param name="comparer">Comparer to use to compare projected values</param>
		/// <returns>The maximal element, according to the projection.</returns>

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null) where TSource : class {
			if (source == null) {
				return default(TSource);
			}
			if (comparer == null) {
				comparer = Comparer<TKey>.Default;
			}
			using (var itor = source.GetEnumerator()) {
				if (!itor.MoveNext()) {
					return default(TSource);
				}
				TSource max = default(TSource);
				TKey maxKey = selector(max);
				while (itor.MoveNext()) {
					var v = itor.Current;
					var p = selector(v);
					if (comparer.Compare(p, maxKey) > 0) {
						max = v;
						maxKey = p;
					}
				}
				return max;
			}
		}

		public static bool IsSame<T>(this IEnumerable<T> source, IEnumerable<T> comparand) {
			if ((object)source == null) {
				return (object)comparand == null;
			}
			if ((object)comparand == null) {
				return false;
			}
			using (var sit = source.GetEnumerator()) {
				using (var cit = comparand.GetEnumerator()) {
					do {
						if (!sit.MoveNext()) {
							return !cit.MoveNext();
						}
						if (!cit.MoveNext()) {
							return false;
						}
					} while (Object.Equals(sit.Current, cit.Current));
					return false;
				}
			}
		}
	}
}
