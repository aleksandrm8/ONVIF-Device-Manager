using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Reflection;
using System.ComponentModel;
using utils;

namespace odm.localization {

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
	public class XPathAttribute : Attribute {
		public XPathAttribute(string xpath) {
			this.xpath = xpath;
		}
		public string xpath;
	}

	public class LocalizedStringsBase<T> : NotifyPropertyChangedBase where T : LocalizedStringsBase<T>, new() {
		protected LinkedList<Action<Func<string, string>>> propEvaluators = new LinkedList<Action<Func<string, string>>>();
		protected LocalizedStringsBase() {
			GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.ForEach(t => {
					var attr = Attribute.GetCustomAttribute(t, typeof(XPathAttribute)) as XPathAttribute;
					if (attr != null) {
						propEvaluators.AddLast((xeval) => {
							t.SetValue(this, xeval(attr.xpath), null);
						});
					}
				});
			SetLocale(Language.Current.CreateEvaluator());
			Language.CurrentObservable.Subscribe(l => {
				SetLocale(l.CreateEvaluator());
			}, err => {
				dbg.Error(err);
			});
		}

		public static T instance = new T();
        public T GetInstance() { return instance; }

		public virtual void SetLocale(Func<string, string> xeval) {
			var _xeval = xeval.Catch(err => {
				dbg.Error(err);
				return null;
			});
			propEvaluators.ForEach(eval => {
				eval(_xeval);
			});
		}

		public void SetLocale(string file) {
			var doc = new XPathDocument(file);
			SetLocale(doc.CreateEvaluator());
		}
	}
}
