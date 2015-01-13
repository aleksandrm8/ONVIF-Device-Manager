using Microsoft.Practices.Unity;
using odm.core;
using odm.ui.core;

namespace odm.ui.viewModels {
	public class DeviceExplorerViewModel : ViewModelDeviceBase {
		public DeviceExplorerViewModel(IUnityContainer container)
			: base(container) {

		}
		public override void Load(INvtSession session, Account account) {
			Current = States.Common;
		}
	}
}
