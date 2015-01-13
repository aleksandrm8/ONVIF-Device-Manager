using System.Windows;
using odm.infra;
using odm.ui.activities;

namespace plugin_manager {
	public interface IVideoSettingsView {
		FrameworkElement CreateView(VideoSettingsView.Model model, IActivityContext<VideoSettingsView.Result> context);
	}
	public class DefaultVideoSettingsView : IVideoSettingsView {
		public FrameworkElement CreateView(VideoSettingsView.Model model, IActivityContext<VideoSettingsView.Result> context) {
			return new VideoSettingsView(model, context);
		}
	}
	//public interface IPluginManager: IVideoSettingsView {
	//    void RegisterVideoSettingsPlugin(IVideoSettingsView vsPlugin);
	//}
	//public class PluginManager : IPluginManager {
	//    public PluginManager() {
	//        PluginvideoSettings = new List<IVideoSettingsView>();
	//    }

	//    List<IVideoSettingsView> PluginvideoSettings;
	//    public void RegisterVideoSettingsPlugin(IVideoSettingsView vsPlugin) {
	//        PluginvideoSettings.Add(vsPlugin);
	//    }
	//    public FrameworkElement GetVideoSettingsView(string[] scopes, IActivityContext<global::odm.ui.activities.VideoSettingsView.Model, global::odm.ui.activities.VideoSettingsView.Result> context) {
	//        FrameworkElement view = null;
	//        if (!AppDefaults.visualSettings.CustomAnalytics_IsEnabled)
	//            return null;
	//        PluginvideoSettings.ForEach(x => {
	//            view = x.GetVideoSettingsView(scopes, context);
	//            return;
	//        });
	//        return view;
	//    }
	//}
}
