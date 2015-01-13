using odm.ui.viewModels;

namespace odm.ui.controls {
	public partial class DigitalIOView : BasePropertyControl {
		public DigitalIOView(DigitalIOViewModel viewModel) {
			InitializeComponent();

            this.DataContext = viewModel;
            
		}
	}
}
