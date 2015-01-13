using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace odm.ui.core {
    public interface IScopesHolder {
        string[] scopes { get; }
    }
    public class ScopesHolder: IScopesHolder {
        public ScopesHolder (string[] sc){
            scopes = sc;
	    }
        public string[] scopes {get; private set;}
    }
}
