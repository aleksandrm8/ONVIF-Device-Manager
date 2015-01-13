using Microsoft.Practices.Unity;
using odm.ui.viewModels;

namespace odm.ui.viewmodels {
    public class AnalyticsViewModel : ViewModelChannelBase {
        public AnalyticsViewModel(IUnityContainer container)
            : base(container) {

	    }
        public override void Load(odm.core.INvtSession session, string chanToken, string profileToken, core.Account account, core.IVideoInfo videoInfo) {
            Current = States.Common;
        }
    }
}
