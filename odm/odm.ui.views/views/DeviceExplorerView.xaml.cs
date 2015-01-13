using System.Windows.Controls;
using odm.ui.viewModels;

namespace odm.ui.views {
	public partial class DeviceExplorerView : UserControl {
		public DeviceExplorerView(DeviceExplorerViewModel viewModel) {
			InitializeComponent();
			this.DataContext = viewModel;
		}
	}
}
