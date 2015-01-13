using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace utils {
	public static class CollectionExtensions {
		
		/// <summary>
		/// projects each element of an array into a new form
		/// </summary>
		/// <typeparam name="TSrc">the type of the elements of source array</typeparam>
		/// <typeparam name="TRes">the type of the value returned by selector</typeparam>
		/// <param name="array">an array of values to invoke a transform function on</param>
		/// <param name="selector">a transform function to apply to each element</param>
		/// <returns>
		/// an array whose elements are the result of invoking the transform function on each element of source array
		/// </returns>
		public static TRes[] Copy<TSrc, TRes>(this TSrc[] array, Func<TSrc, TRes> selector) {
			if(array == null){
				return null;
			}
			var cnt = array.Length;
			var mappedArray = new TRes[cnt];
			for (int i = 0; i < cnt; ++i) {
				mappedArray[i] = selector(array[i]);
			}
			return mappedArray;
		}

		/// <summary>
		/// expand array on specified number of elements
		/// </summary>
		/// <typeparam name="T">type of elements</typeparam>
		/// <param name="array">array to be expanded</param>
		/// <param name="capacity">number of elements that will be added to new array</param>
		/// <returns>
		/// if capacity is lower or equal zero the array passsed as argumentt will be returned.
		/// otherwise returns new array with copy of elements from the array passed as argument, 
		/// and padded with number of elements specified by capacity argument
		/// </returns>
		public static T[] Expand<T>(this T[] array, int capacity) {
			if (capacity <= 0) {
				return array;
			}
			if (array == null) {
				return new T[capacity];
			}
			var newArray = new T[array.Length + capacity];
			array.CopyTo(newArray, 0); 
			Array.Copy(array, newArray, array.Length);
			return newArray;
		}

		/// <summary>
		/// copy all elements from linked list to array
		/// </summary>
		/// <typeparam name="T">element type</typeparam>
		/// <param name="list">list which elements will be copied to array</param>
		/// <returns>array filled with elements from list, or null if list was null</returns>
		public static T[] ToArray<T>(this LinkedList<T> list) {
			if (list == null) {
				return null;
			}
			var cnt = list.Count;
			if (cnt <= 0) {
				return new T[0];
			}
			var array = new T[cnt];
			int i = 0;
			foreach (var e in list) {
				array[i] = e;
				++i;
			}
			return array;
		}

		/// <summary>
		/// adds specified elemts at end of the list
		/// </summary>
		/// <typeparam name="T">type of elements</typeparam>
		/// <param name="list">list where elements will be added</param>
		/// <param name="elements">elements to be added to the end of list</param>
		/// <returns>
		/// the list that was passed as argument with added elemets if it's not null, 
		/// null if both list and elements where null,
		/// new list filled with specified elements, if the passed list was null and elements wasn't null
		/// </returns>
		public static LinkedList<T> Concat<T>(this LinkedList<T> list, IEnumerable<T> elements) {
			if (elements == null) {
				return list;
			}
			if (list == null) {
				return new LinkedList<T>(elements);
			}
			foreach (var e in elements) {
				list.AddLast(e);
			}
			return list;
		}

	}
}
