using odm.ui.viewModels;
using utils;
using System.Windows.Controls;

namespace odm.ui.controls {
	public partial class SystemLogView : BasePropertyControl {
        public SystemLogView(SystemLogViewModel viewModel) {
			InitializeComponent();

            this.DataContext = viewModel;

			btnSaveAttach.CreateBinding(Button.ToolTipProperty, LocalSystemLog.instance, s => s.titleSaveAttach);
			btnSaveLog.CreateBinding(Button.ToolTipProperty, LocalSystemLog.instance, s => s.titleSaveLog);
		}
	}
}
