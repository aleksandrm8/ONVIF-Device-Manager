using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using odm.core;
using odm.ui.controls;
using odm.ui.core;
using onvif.services;

namespace odm.ui.viewModels {
    public class ProfilesViewModel:DependencyObject, IDisposable {
		public ProfilesViewModel(IUnityContainer container, string systemProfile, String chanToken) {
            Profiles = new ObservableCollection<Profile>();
            this.eventAggregator = container.Resolve<EventAggregator>();
            CurrentSession = container.Resolve<INvtSession>();
            //ChannelToken = container.Resolve <VideoSourceToken>();
            SystemProfile = systemProfile;

            InitCommands();
        }
        
        public PropertyProfileEditorStrings Strings { get { return PropertyProfileEditorStrings.instance; } }
        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }

		public ObservableCollection<Profile> Profiles { get; set; }
        List<Profile> OriginProfilesList = new List<Profile>();
        public string SystemProfile { get; set; }
        EventAggregator eventAggregator;
        INvtSession CurrentSession;

        //void SelectProfile() {
        //    ProfileChangedEventArgs evargs = new ProfileChangedEventArgs(CurrentSession, ChannelToken, SelectedProfile.token);
        //    eventAggregator.GetEvent<ProfileChangedPreviewEvent>().Publish(evargs);
        //}

		public Action<string> DeleteProfile;
        public Action<string> NewProfile;
		public Action<string> SelectProfile;

        #region Commands
        bool CanSelect() {
            return SelectedProfile != null && SelectedProfile.token != SystemProfile;
        }
        bool CanDelete() { 
            return SelectedProfile != null && SelectedProfile.token != SystemProfile;
        }
        public void InitCommands() {
            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(SelectedProfileProperty, typeof(ProfilesViewModel));
            descriptor.AddValueChanged(this, (obj, evarg) => {
                delete.CanExecuteDelegate(null);
                select.CanExecuteDelegate(null);
            });

            select = new SimpleCommand(canexecute => {
                return CanSelect();
            }, execute => {
                SelectProfile(SelectedProfile.token);
            });
            delete = new SimpleCommand(canexecute => {
                return CanDelete();
            }, execute => {
                DeleteProfile(SelectedProfile.token);
            });
            create = new SimpleCommand(canexecute => {
                return true;
            }, execute => {
                NewProfile(ProfileName);
            });
        }

        public SimpleCommand select {
            get { return (SimpleCommand)GetValue(selectProperty); }
            set { SetValue(selectProperty, value); }
        }
        public static readonly DependencyProperty selectProperty = DependencyProperty.Register("select", typeof(SimpleCommand), typeof(ProfilesViewModel));

        public SimpleCommand delete {
            get { return (SimpleCommand)GetValue(deleteProperty); }
            set { SetValue(deleteProperty, value); }
        }
        public static readonly DependencyProperty deleteProperty = DependencyProperty.Register("delete", typeof(SimpleCommand), typeof(ProfilesViewModel));

        public SimpleCommand create {
            get { return (SimpleCommand)GetValue(createProperty); }
            set { SetValue(createProperty, value); }
        }
        public static readonly DependencyProperty createProperty = DependencyProperty.Register("create", typeof(SimpleCommand), typeof(ProfilesViewModel));

		public Profile SelectedProfile {
			get { return (Profile)GetValue(SelectedProfileProperty); }
            set { SetValue(SelectedProfileProperty, value); }
        }
        public static readonly DependencyProperty SelectedProfileProperty =
			DependencyProperty.Register("SelectedProfile", typeof(Profile), typeof(ProfilesViewModel));

        public string ProfileName {
            get { return (string)GetValue(ProfileNameProperty); }
            set { SetValue(ProfileNameProperty, value); }
        }
        public static readonly DependencyProperty ProfileNameProperty =
            DependencyProperty.Register("ProfileName", typeof(string), typeof(ProfilesViewModel));
        #endregion

		public void Dispose() {
		}
	}
}
