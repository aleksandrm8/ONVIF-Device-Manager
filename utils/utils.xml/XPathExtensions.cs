using System;
using System.Text;
using System.Xml.XPath;

namespace utils {
	public static class XPathNavigable {
		private class AnonymousXPathNavigable : IXPathNavigable {
			private Func<XPathNavigator> m_factory;
			public AnonymousXPathNavigable(Func<XPathNavigator> factory) {
				if (factory == null) {
					throw new ArgumentNullException("factory");
				}
				m_factory = factory;
			}

			public XPathNavigator CreateNavigator() {
				return m_factory();
			}
		}
		public static IXPathNavigable Create(Func<XPathNavigator> factory) {
			return new AnonymousXPathNavigable(factory);
		}
	}

	public static class XPathExtensions {
		public static Func<string, string> CreateEvaluator(this IXPathNavigable navigable) {
			return navigable.CreateNavigator().GetEvaluator();
		}
		public static Func<XPathExpression, string> CreateExprEvaluator(this IXPathNavigable navigable) {
			return navigable.CreateNavigator().GetExprEvaluator();
		}

		public static Func<string, string> GetEvaluator(this XPathNavigator navigator) {
			var xeval = GetExprEvaluator(navigator);
			return xpath => {
				XPathExpression expr = null;
				expr = XPathExpression.Compile(xpath);
				return xeval(expr);
			};
		}

		public static Func<XPathExpression, string> GetExprEvaluator(this XPathNavigator navigator) {
			return xpath => {
				if (navigator == null) {
					return null;
				}
				var t = navigator.Select(xpath);
				var sb = new StringBuilder();
				while (t.MoveNext()) {
					sb.Append(t.Current);
				}
				var result = sb.ToString();
				if (String.IsNullOrWhiteSpace(result)) {
					return null;
				}
				return result;
			};
		}
	}
}
