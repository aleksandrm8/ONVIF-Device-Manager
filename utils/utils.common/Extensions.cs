using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Collections.ObjectModel;

namespace utils {


	public static class ObservableExtensions {
		public static IObservable<object> Handle<T>(this IObservable<T> observable, Action<T> act) {
			return observable.Do(act).TakeLast(0).Select(x => new object());
		}

		public static IObservable<object> Idle<T>(this IObservable<T> observable) {
			return Observable.Create<Object>(observer => {
				return observable.Subscribe(_ => {
				}, observer.OnError, observer.OnCompleted);
			});
		}

		public static IObservable<T> IgnoreError<T>(this IObservable<T> observable) {
			return Observable.Create<T>(observer => {
				return observable.Subscribe(observer.OnNext, err => observer.OnCompleted(), observer.OnCompleted);
			});
		}
		public static IObservable<T> HandleError<T>(this IObservable<T> observable, Action<Exception> errorHandler) {
			return Observable.Create<T>(observer => {
				return observable.Subscribe(observer.OnNext,
					err => {
						try {
							errorHandler(err);
						} finally {
							observer.OnCompleted();
						}
					},
					observer.OnCompleted);
			});
		}
	}


	public static class Extensions {

		public static Stream ToStream(this byte[] byteArray) {
			if (byteArray == null) {
				return null;
			}
			return new MemoryStream(byteArray);
		}

		public static Func<TArg, TResult> Wrap<TArg, TResult>(this Func<TArg, TResult> fun, Func<Func<TArg, TResult>, Func<TArg, TResult>> wrapper) {
			return wrapper(fun);
		}

		public static void Wrap<TArg>(this Action<TArg> act, Action<Action<TArg>> wrapper) {
			wrapper(act);
		}

		public static Func<TArg, TResult> Catch<TArg, TResult>(this Func<TArg, TResult> fun, Func<TResult> handler) {
			return arg => {
				try {
					return fun(arg);
				} catch {
					return handler();
				}
			};
		}

		public static Func<TArg, TResult> Catch<TArg, TResult>(this Func<TArg, TResult> fun, Func<Exception, TResult> handler) {
			return arg => {
				try {
					return fun(arg);
				} catch (Exception err) {
					return handler(err);
				}
			};
		}

		public static T GetCustomAttribute<T>(this Type type) where T : Attribute {
			return Attribute.GetCustomAttribute(type, typeof(T)) as T;
		}

		//public static void SetDoubleBuffered(this System.Windows.Forms.Control control, bool value){
		//    var type = control.GetType();
		//    var prop = type.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		//    prop.SetValue(control, value, null);
		//    //throw new NotImplementedException("SetDoubleBuffered");
		//}

		//public static void SetDoubleBufferedRecursive(this System.Windows.Forms.Control control, bool value){
		//    control.SetDoubleBuffered(value);
		//    control.Controls.ForEach(_control => {
		//        SetDoubleBufferedRecursive(_control as System.Windows.Forms.Control, value);				
		//    });				
		//}

		//private static IObservable<T> OnError<T>(this IObservable<T> observable, Action<Exception> errorHandler) {
		//    return Observable.CreateWithDisposable<T>(observer => {
		//        var subscription = new MutableDisposable();
		//        var node = slot.Add(subscription);
		//        subscription.Disposable = observable
		//            .Subscribe(
		//                (t) => observer.OnNext(t),
		//                (err) => observer.OnError(err),
		//                () => {
		//                    slot.Complete();
		//                    observer.OnCompleted();
		//                });
		//        return slot;
		//    });
		//}

		public static IObservable<T> OnCompleted<T>(this IObservable<T> observable, Action completeHandler) {
			if (completeHandler == null) {
				throw new ArgumentNullException("completeHandler");
			}
			return Observable.Create<T>(observer => {
				var subscription = observable
					.Subscribe(
						(t) => {
							observer.OnNext(t);
						},
						(err) => {
							observer.OnError(err);
						},
						() => {
							completeHandler();
							observer.OnCompleted();
						});
				return subscription;
			});
		}
		public static IObservable<T> OnError<T>(this IObservable<T> observable, Action<Exception> errorHandler) {
			if (errorHandler == null) {
				throw new ArgumentNullException("errorHandler");
			}
			return Observable.Create<T>(observer => {
				var subscription = observable
					.Subscribe(
						(t) => observer.OnNext(t),
						(err) => {
							errorHandler(err);
							observer.OnError(err);
						},
						() => observer.OnCompleted()
					);
				return subscription;
			});
		}

		public static IObservable<T> OnDispose<T>(this IObservable<T> observable, Action disposeHandler) {
			if (disposeHandler == null) {
				throw new ArgumentNullException("disposeHandler");
			}
			return Observable.Create<T>(observer => {
				var subscription = observable.Subscribe(observer);
				return Disposable.Create(() => {
					disposeHandler();
					subscription.Dispose();
				});

			});
		}

		public static void Add(this CompositeDisposable compositeDisposable, Action disposeHandler) {
			compositeDisposable.Add(Disposable.Create(disposeHandler));
		}

		public static Uri GetBaseUri(this Uri uri) {
			if (!uri.IsAbsoluteUri) {
				return null;
			}
			var ub = new UriBuilder(uri.Scheme, uri.Host, uri.Port);
			ub.UserName = uri.UserInfo;
			return ub.Uri;
		}
		public static Uri Relocate(this Uri uri, string host) {
			if (!uri.IsAbsoluteUri) {
				return uri;
			}
			var ub = new UriBuilder(uri);
			ub.UserName = uri.UserInfo;
			ub.Password = null;
			ub.Host = host;
			return ub.Uri;
		}
		public static Uri Relocate(this Uri uri, string host, int port) {
			if (!uri.IsAbsoluteUri) {
				return uri;
			}
			var ub = new UriBuilder(uri);
			ub.UserName = uri.UserInfo;
			ub.Password = null;
			ub.Host = host;
			ub.Port = port;
			return ub.Uri;
		}

		public static Uri Append(this Uri baseUri, string relative) {
			if(String.IsNullOrWhiteSpace(relative)){
				return baseUri;
			}
			var relUri = new Uri(relative, UriKind.RelativeOrAbsolute);
			string relQuery;
			string relPath;

			if (!relUri.IsAbsoluteUri) {
				var i = relative.IndexOf('?');
				if (i > 0) {
					relQuery = relative.Substring(i + 1);
					relPath = relative.Substring(0, i);
				} else {
					relQuery = null;
					relPath = relative;
				}
			} else {
				relQuery = relUri.GetComponents(UriComponents.Query, UriFormat.Unescaped);
				relPath = relUri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
			}
			
			string baseQuery;
			if (!baseUri.IsAbsoluteUri) {
				string basePath; 
				var i = baseUri.OriginalString.IndexOf('?');
				if (i > 0) {
					baseQuery = baseUri.OriginalString.Substring(i + 1);
					basePath = baseUri.OriginalString.Substring(0, i);
				} else {
					baseQuery = null;
					basePath = baseUri.OriginalString;
				}
				var query = String.Join("&", new[] { baseQuery, relQuery}.Where(s => !String.IsNullOrWhiteSpace(s)));
				var path = String.Join("/", new[] { basePath.TrimEnd('/'), relPath.TrimStart('/') }.Where(s => !String.IsNullOrWhiteSpace(s)));

				if (String.IsNullOrWhiteSpace(query)) {
					return new Uri(path, UriKind.RelativeOrAbsolute);
				} else {
					return new Uri(path + "?" + query, UriKind.RelativeOrAbsolute);
				}
			} else {
				baseQuery = baseUri.GetComponents(UriComponents.Query, UriFormat.Unescaped);
			}

			var ub = new UriBuilder(baseUri);
			ub.Path = String.Join("/", new[] { ub.Path.TrimEnd('/'), relPath.TrimStart('/') }.Where(s => !String.IsNullOrWhiteSpace(s)));
			ub.Query = String.Join("&", new[] { baseQuery, relQuery}.Where(s => !String.IsNullOrWhiteSpace(s)));
			return ub.Uri;
		}

		/// <summary>
		/// adds element at end of the list and shifts it left unless the list length is lower or equal than sizeMax
		/// </summary>
		/// <typeparam name="E">list elemnet type</typeparam>
		/// <param name="list">list where element is pushed</param>
		/// <param name="elem">element to push</param>
		/// <param name="sizeMax">maximum length of list after element was pushed</param>
		public static void ShiftPush<E>(this IList<E> list, E elem, int sizeMax) {
			if (sizeMax < 0) {
				throw new ArgumentOutOfRangeException("sizeMax");
			}
			list.Add(elem);
			while (list.Count > sizeMax) {
				list.RemoveAt(0);
			}
		}

        /// <summary>
        /// adds element at begin of the list and shifts it right unless the list length is lower or equal than sizeMax
        /// </summary>
        /// <typeparam name="E">list elemnet type</typeparam>
        /// <param name="list">list where element is pushed</param>
        /// <param name="elem">element to push</param>
        /// <param name="sizeMax">maximum length of list after element was pushed</param>
        public static void ShiftPushAtBeginnig<E>(this IList<E> list, E elem, int sizeMax) {
            if (sizeMax < 0) {
                throw new ArgumentOutOfRangeException("sizeMax");
            }
            list.Insert(0, elem);
            while (list.Count > sizeMax) {
                list.RemoveAt(list.Count - 1);
            }
        }

		/// <summary>
		/// checks if status code is successful
		/// </summary>
		/// <param name="statusCode">status code</param>
		/// <returns>true if status is successful, false otherwise </returns>
		public static bool IsSuccessful(this HttpStatusCode statusCode) {
			var i = (int)statusCode;
			return i >= 200 && i < 300;
		}


        public static void AddRange<T>(this ICollection<T> container, IEnumerable<T> items) {
            foreach (var item in items)
                container.Add(item);
        }
        /// <summary>
        /// container should contain only unique items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="items"></param>
        public static void RemoveRange<T>(this ICollection<T> container, IEnumerable<T> items) {
            foreach (var item in items)
                container.Remove(item);
        }
	}	

}
