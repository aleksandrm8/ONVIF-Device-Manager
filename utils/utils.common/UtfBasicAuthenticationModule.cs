using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Globalization;

namespace utils {

	public class UtfBasicAuthenticationModule: IAuthenticationModule {
		
		const string auth_type = "Basic";

		public string AuthenticationType {
			get { return "Basic"; }
		}

		public bool CanPreAuthenticate {
			get { return true; }
		}

		public Authorization PreAuthenticate(WebRequest request, ICredentials credentials) {
			if (credentials == null) {
				return null;
			}
			HttpWebRequest httpWebRequest = request as HttpWebRequest;
			if (httpWebRequest == null) {
				return null;
			}
			return this.Lookup(httpWebRequest, credentials);
		}

		public Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials) {
			if (credentials == null) {
				return null;
			}
			HttpWebRequest httpWebRequest = request as HttpWebRequest;
			if (httpWebRequest == null || httpWebRequest.RequestUri == null) {
				return null;
			}
			if(!challenge.Trim().StartsWith("Basic", true, CultureInfo.InvariantCulture)){
				return null;
			}
			return this.Lookup(httpWebRequest, credentials);
		}

		private Authorization Lookup(HttpWebRequest httpWebRequest, ICredentials credentials) {
			//NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, BasicClient.Signature);
			var net_cred = credentials.GetCredential(httpWebRequest.RequestUri, "Basic");
			if (net_cred == null) {
				return null;
			}
			ICredentialPolicy credentialPolicy = AuthenticationManager.CredentialPolicy;
			if (credentialPolicy != null && !credentialPolicy.ShouldSendCredential(httpWebRequest.RequestUri, httpWebRequest, net_cred, this)) {
				return null;
			}
			var user_pass = String.Join(":", net_cred.UserName, net_cred.Password).ToUtf8();
			string authToken = "Basic " + user_pass.ToBase64();
			return new Authorization(authToken, true);
		}
	}
}
