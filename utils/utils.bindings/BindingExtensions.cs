using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;

namespace utils {
	public static class BindingExtensions {
		public static IObservable<TProp> GetPropertyChangedEvents<TModel, TProp>(this TModel model, Expression<Func<TModel, TProp>> propExpr) 
			where TModel : INotifyPropertyChanged {
			var prop_member = propExpr.Body as MemberExpression;
			dbg.Assert(prop_member != null);
			var member_name = prop_member.Member.Name;
			Func<TModel, TProp> getVal = null;
			if(prop_member.Member.MemberType == MemberTypes.Property){
				var info = prop_member.Member as PropertyInfo;
				getVal = m => (TProp)info.GetValue(model,null);
			}else if(prop_member.Member.MemberType == MemberTypes.Field){
				var info = prop_member.Member as FieldInfo;
				getVal = m => (TProp)info.GetValue(model);
			}
			
			if(getVal == null){
				var err = new ArgumentException("invalid property expression");
				dbg.Error(err);
				throw err;
			}
			
			return Observable.Create<TProp>(observer => {
				PropertyChangedEventHandler handler = (sender, args) => {
					try {
						if (args == null || args.PropertyName == member_name) {
							observer.OnNext(getVal(model));
						}
					} catch (Exception err) {
						dbg.Error(err);
						//swallow error
					}
				};				
				model.PropertyChanged += handler;
				handler(model, null);
				return Disposable.Create(() => {
					model.PropertyChanged -= handler;
				});
			});
		}
	}			

}
