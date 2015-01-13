#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

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
using System.IO.MemoryMappedFiles;

using odm.models;
using odm.utils.extensions;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyLiveVideo.xaml
	/// </summary>
	public partial class PropertyLiveVideo : BasePropertyControl {
		public PropertyLiveVideo(LiveVideoModel devModel, MemoryMappedFile memFile, Action<string> SetRecordFolder, Action StartRecording, Action StopRecording) {
			InitializeComponent();
			OnStartRecording = StartRecording;
			OnStopRecording = StopRecording;
			OnSetRecordFolder = SetRecordFolder;
			_devModel = devModel;
			_memFile = memFile;

			_videoPlayer.memFile = memFile;
			InitControls();
			_tbVideoUrl.Text = devModel.mediaUri;

			Localization();
		}
		Action OnStartRecording;
		Action OnStopRecording;
		Action<string> OnSetRecordFolder;
		string savingPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\ONVIFVideo";
		LiveVideoModel _devModel;
		MemoryMappedFile _memFile;
		LinkButtonsStrings titles = new LinkButtonsStrings();
		PropertyLiveVideoStrings strings = new PropertyLiveVideoStrings();

		void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.liveVideo);
			selectSavePath.CreateBinding(Button.ContentProperty, strings, x => x.folder);
		}
		void InitControls() { 
			Rect ret = new Rect(0,0,_devModel.encoderResolution.Width, _devModel.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);

			if (!odm.ui.Properties.Settings.Default.UseDefaultRecordingPath) {
				savingPath = odm.ui.Properties.Settings.Default.RecordingPath;
				savePath.Text = savingPath;
				OnSetRecordFolder(savingPath);
			} else {
				savePath.Text = savingPath;
				OnSetRecordFolder(savingPath);
			}

			selectSavePath.Click += new RoutedEventHandler(selectSavePath_Click);
			checkBox1.Click += new RoutedEventHandler(checkBox1_Click);
		}

		void selectSavePath_Click(object sender, RoutedEventArgs e) {
			System.Windows.Forms.FolderBrowserDialog fdlg = new System.Windows.Forms.FolderBrowserDialog();
			fdlg.ShowNewFolderButton = true;
			fdlg.ShowDialog();
			string path = fdlg.SelectedPath;
			savePath.Text = path;
			odm.ui.Properties.Settings.Default.UseDefaultRecordingPath = false;
			odm.ui.Properties.Settings.Default.RecordingPath = path;
			odm.ui.Properties.Settings.Default.Save();
			OnSetRecordFolder(path);
		}

		void checkBox1_Click(object sender, RoutedEventArgs e) {
			if (checkBox1.IsChecked.Value) {
				if (OnStartRecording != null) {
					OnStartRecording();
				}
			} else {
				if (OnStopRecording != null) {
					OnStopRecording();
				}
			}

		}
		public override void ReleaseAll() {
			if (OnStopRecording != null)
				OnStopRecording();
			_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
	}
}
