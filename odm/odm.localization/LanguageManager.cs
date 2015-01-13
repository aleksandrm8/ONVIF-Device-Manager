using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Globalization;

using System.Reactive.Linq;
using System.Reactive.Subjects;

using utils;

namespace odm.localization {
	
	public class Language:IXPathNavigable{
		private Language() {
            DisplayName = null;
            iso3 = null;
		}
		private string m_FilePath = null;

		public string DisplayName{get;set;}
		public string iso3{get;set;}

		public virtual XPathNavigator CreateNavigator() {
			if (String.IsNullOrEmpty(m_FilePath)) {
				return null;
			}
			return new XPathDocument(m_FilePath).CreateNavigator();
		}

		public static IEnumerable<Language> AvailableLanguages {
			get {
				//yield return Default;
				var langs = Bootstrapper.specialFolders.locales
					.GetFiles("*.xml")
					//.Select(x => new FileInfo(x))
					.Select(x => new Language() {
						DisplayName = Path.GetFileNameWithoutExtension(x.Name),
						m_FilePath = x.FullName
					});
				foreach (var t in langs) {
					try {
						var xeval = t.CreateEvaluator();
						var lang_name = xeval("/localized-strings/@name");
						var lang_iso3 = xeval("/localized-strings/@lang-iso3");
						
						if (lang_name == null && lang_iso3!=null) {
							var ci = CultureInfo.GetCultures(CultureTypes.NeutralCultures)
								.Where(c => c.ThreeLetterISOLanguageName.Equals(lang_iso3, StringComparison.OrdinalIgnoreCase))
								.FirstOrDefault();
							lang_name = ci != null ? ci.NativeName.ToLower() : null;
						}

						t.iso3 = lang_iso3;
						t.DisplayName = lang_name;
					} catch(Exception err) {
						//swallow error
						dbg.Error(err);
					}
					yield return t;
				}
				
			}
		}

		public static Language Default = new Language() {
			DisplayName = "english(default)",
			iso3 = "eng"
		};
		private static Language m_Current = null;
		public static Language Current {
			get {
				if (m_Current == null) {
					return Default;
				}
				return m_Current;
			}
			set {
				if(m_Current != value){
					m_Current = value;
					m_CurrentObservable.OnNext(value);
				}
			}
		}
		private static Subject<Language> m_CurrentObservable = new Subject<Language>();
		public static IObservable<Language> CurrentObservable {
			get {
				return m_CurrentObservable;
			}
		}
	}

	//class LanguageManager {
	//    public static IEnumerable<LanguageData> AvailableLanguages;
	//    public static LanguageData GetCurrentLanguage() {
	//        throw new NotImplementedException();
	//    }
	//    public static void SetCurrentLanguage(LanguageData language) {
	//        throw new NotImplementedException();
	//    }
	//    public static IObservable<LanguageData> CurrentLanguage;
	//}
}
