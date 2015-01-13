using odm.ui.controls;
using odm.ui.viewModels;

namespace odm.views {
	public partial class ProfilesView : BasePropertyControl {
		public ProfilesView(ProfilesViewModel viewModel) {
            InitializeComponent();
            this.DataContext = viewModel;
         
		}
	}
}
