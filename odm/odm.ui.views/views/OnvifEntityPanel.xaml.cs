using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using odm.ui.core;
using odm.core;
using utils;
using System.Reactive.Disposables;
using Microsoft.Practices.Unity;
using odm.ui.controls;
using odm.ui.activities;
using odm.ui.links;
using Microsoft.Practices.Prism.Events;

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for OnvifEntityPanel.xaml
	/// </summary>
	public partial class OnvifEntityPanel : UserControl, IDisposable{
		public OnvifEntityPanel() {
			InitializeComponent();
		}

		CompositeDisposable disposables = new CompositeDisposable();
		INvtSession invtSession;
		IUnityContainer container;

		public void Init(DeviceDescriptionHolder devHolder, NvtSessionFactory sessionFactory, IUnityContainer container) {
			this.container = container;
			//Add device section (all devices must have this section)
			parent.Title = devHolder.Name;
			
			//Display progress bar
			devicePanel.Content = new ProgressView("Loading ...");

			//Begin load device section
			disposables.Add(SectionDevice.Load(devHolder, sessionFactory)
				.ObserveOnCurrentDispatcher()
				.Subscribe(
					args => {
						invtSession = args.nvtSession;

						SectionDevice devView = container.Resolve<SectionDevice>();
						disposables.Add(devView);
						devView.Init(args);
						devicePanel.Content = devView;

						//Load sections
						LoadSections(args);
					}, 
					err => {
						ErrorView errorView = new ErrorView(err);
						disposables.Add(errorView);

						devicePanel.Content = errorView;
					}
			));

		}

		List<Action<DeviceViewArgs, bool>> priority = new List<Action<DeviceViewArgs, bool>>();
		List<SectionPanel> sections = new List<SectionPanel>();

		void LoadSections(DeviceViewArgs args) {
			//investigate capability to find what sections is available
			if (args.capabilities.media != null) { 
				//nvt section on
				priority.Add(LoadNvtSection);
			}
			if (args.capabilities.extension != null && args.capabilities.extension.analyticsDevice != null) {
				priority.Add(LoadNvaSection);
			}

			for(int i=0; i<priority.Count; i++){
				priority[i](args, i == 0);
			}
		}
		void LoadNvtSection(DeviceViewArgs args, bool isActive) {
			SourceSectionPanel spanel = new SourceSectionPanel(args, container);
			spanel.Init(isActive, null, () => {
				sections.ForEach(sp => {
					if (sp != spanel)
						sp.Hide();
				});
			});
			sections.Add(spanel);
			
			spanel.header.Content = "NVT";
			onvifEntitiesPanel.Children.Add(spanel);

			spanel.CreateView(args, container);
		}
		void LoadNvaSection(DeviceViewArgs args, bool isActive) {
			AnalyticsSectionPanel spanel = new AnalyticsSectionPanel(args, container);
			spanel.Init(isActive, null, () => {
				sections.ForEach(sp => {
					if (sp != spanel)
						sp.Hide();
				});
			});
			sections.Add(spanel);

			spanel.header.Content = "NVA";
			onvifEntitiesPanel.Children.Add(spanel);

			spanel.CreateView(args, container);
		}
		void LoadNvrSection(DeviceViewArgs args, bool isActive) {
		}
		void LoadDisplaySection(DeviceViewArgs args, bool isActive) {
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
