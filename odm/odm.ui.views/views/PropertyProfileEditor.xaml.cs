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
using odm.ui.controls;
using onvif.services.media;
using odm.utils.extensions;
using System.Collections.ObjectModel;
using odm.utils;
using onvif;

namespace odm.views {
	/// <summary>
	/// Interaction logic for PropertyProfileEditor.xaml
	/// </summary>
	public partial class PropertyProfileEditor : BasePropertyControl {
		public PropertyProfileEditor(List<Profile> plst, ProfileToken profileToken, Action createNew, Action<Profile> renameProfile, Action<Profile> selectProfile, Action<Profile> deleteProfile) {
			profCollection = new ObservableCollection<Profile>();
			InitializeComponent();

            profilesList.SelectionMode = SelectionMode.Single;

			onCreateNew = createNew;
			onRenameProfile = renameProfile;
			onDeleteProfile = deleteProfile;
			onSelectProfile = selectProfile;
			Localization();
			InitControl();
			BindData(plst, profileToken);
		}
		Action onCreateNew;
		Action<Profile> onRenameProfile;
		Action<Profile> onDeleteProfile;
		Action<Profile> onSelectProfile;
		PropertyProfileEditorStrings strings = new PropertyProfileEditorStrings();
		LinkButtonsStrings titles = new LinkButtonsStrings();
		public ObservableCollection<Profile> profCollection {
			get;
			set;
		}
		public Profile currentProf = new Profile();
		public Profile selectedProf = new Profile();
		public string resolution {
			get {
				string val = selectedProf.VideoEncoderConfiguration.Resolution.Width.ToString() + 'x' + selectedProf.VideoEncoderConfiguration.Resolution.Height.ToString();
				return val;
			}
		}

		void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.profileEditor);

			//setProfileName.CreateBinding(Button.ContentProperty, strings, x => x.rename);
			deleteSelected.CreateBinding(Button.ContentProperty, strings, x => x.delete);
			createNew.CreateBinding(Button.ContentProperty, strings, x => x.newProf);
			selectCurrent.CreateBinding(Button.ContentProperty, strings, x => x.select);
		}
		void BindData(List<Profile> plst, ProfileToken tok) {
			plst.ForEach(x =>
				profCollection.Add(x)
				);

			//profilesList.ItemsSource = profCollection;
			//profilesList.DisplayMemberPath = "Name";
			
			profilesList.SelectionChanged += (object sender, SelectionChangedEventArgs e) => {
                if (profilesList.SelectedItem != null)
                {
                    if (profilesList.SelectedItem == currentProf || ((Profile)profilesList.SelectedItem).@fixed)
                        deleteSelected.IsEnabled = false;
                    else
                        deleteSelected.IsEnabled = true;
                }
                else
                {
                    deleteSelected.IsEnabled = false;
                }
			};
			
			try {
				currentProf = plst.Find(x => x.token == tok);
				selectedProf = currentProf;
				profilesList.SelectedItem = currentProf;
			} catch (Exception err) {
				dbg.Error(err);
			}
		}

		public void AddProfile(Profile prof) {
			profCollection.Add(prof);
		}
		public void DeleteProfile(Profile prof) {
			profCollection.Remove(prof);
		}

		void InitControl() {
			selectCurrent.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => {
				if (onSelectProfile != null) onSelectProfile((Profile)profilesList.SelectedItem);
			});
			createNew.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => {
				if (onCreateNew != null) onCreateNew();
			});
			deleteSelected.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => {
				if (onDeleteProfile != null) onDeleteProfile((Profile)profilesList.SelectedItem);
			});
			rename.IsEnabled = false;
			rename.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => {
				if (onRenameProfile != null) {
					((Profile)profilesList.SelectedItem).Name = profileNameEditor.Text;
					onRenameProfile((Profile)profilesList.SelectedItem); 
				}
			});
		}

		public override void ReleaseAll() {
			base.ReleaseAll();
		}
	}
}
