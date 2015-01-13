using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Practices.Unity;

namespace odm.extensibility {
	[ComImport]
	[Guid("0A4CC32A-1997-4EBD-96C8-FF8D96D9B7A9")]
	//[InheritedExport]
	public interface IPlugin {
		void OnDeviceSettingsContextCreated(IUnityContainer container);
		void Init();
	}
}
