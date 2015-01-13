using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using odm.models;
using odm.core;
using utils;
using odm.infra;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Practices.Prism.Commands;

namespace odm.ui.core {
    public class DeviceDescriptionHolder:DependencyObject {
        public DeviceDescriptionHolder() {
			deleteDevice = new DelegateCommand(() => {
				if (OnDelete != null)
					OnDelete(this);
			});
			editDeviceUri = new DelegateCommand(() => {
				if (OnEdit != null) {
					OnEdit(this);
				}
			});
        }

		public bool IsInvalidUris = false;

#region ManualAdd
 		public void FillCommands(Action<DeviceDescriptionHolder> OnDelete, Action<DeviceDescriptionHolder> OnEdit) {
			this.OnDelete = OnDelete;
			this.OnEdit = OnEdit;
		}
		Action<DeviceDescriptionHolder> OnDelete; 
		Action<DeviceDescriptionHolder> OnEdit;

		public bool IsManual { get; set; }
		public ManualDevice ManualDevice { get; set; }

		public DelegateCommand deleteDevice {
			get { return (DelegateCommand)GetValue(deleteDeviceProperty); }
			set { SetValue(deleteDeviceProperty, value); }
		}
		public static readonly DependencyProperty deleteDeviceProperty = DependencyProperty.Register("deleteDevice", typeof(DelegateCommand), typeof(DeviceDescriptionHolder));

		public DelegateCommand editDeviceUri {
			get { return (DelegateCommand)GetValue(editDeviceUriProperty); }
			set { SetValue(editDeviceUriProperty, value); }
		}
		public static readonly DependencyProperty editDeviceUriProperty = DependencyProperty.Register("editDeviceUri", typeof(DelegateCommand), typeof(DeviceDescriptionHolder));
#endregion

        public void Init(IChangeTrackable<IIdentificationModel> model) {
            this.CreateBinding(NameProperty, model.current, x => x.name);
            this.CreateBinding(LocationProperty, model.current, x => x.location);
            this.CreateBinding(DeviceIconUriProperty, model.current, x => x.iconUri); 
            this.CreateBinding(FirmwareProperty, model.current, x => x.firmware);
			//this.CreateBinding(AddressProperty, model.current, x => x.ip);//return session.deviceUri.Host;
			if (session != null && session.deviceUri != null)
				Address = session.deviceUri.Host;
			else {
				Address = model.current.ip;
			}
            this.CreateBinding(ManufacturerProperty, model.current, x => x.manufacturer);
            this.CreateBinding(DeviceModelProperty, model.current, x => x.model);
        }

        public System.Net.NetworkCredential Account { get; set; }

        public Uri[] Uris { get; set; }
        public string Address {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Address.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(DeviceDescriptionHolder));

        public string DeviceModel {
            get { return (string)GetValue(DeviceModelProperty); }
            set { SetValue(DeviceModelProperty, value); }
        }
        public static readonly DependencyProperty DeviceModelProperty =
            DependencyProperty.Register("DeviceModel", typeof(string), typeof(DeviceDescriptionHolder));

        public string Manufacturer {
            get { return (string)GetValue(ManufacturerProperty); }
            set { SetValue(ManufacturerProperty, value); }
        }
        public static readonly DependencyProperty ManufacturerProperty =
            DependencyProperty.Register("Manufacturer", typeof(string), typeof(DeviceDescriptionHolder));

        public string Firmware {
            get { return (string)GetValue(FirmwareProperty); }
            set { SetValue(FirmwareProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Firmware.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirmwareProperty =
            DependencyProperty.Register("Firmware", typeof(string), typeof(DeviceDescriptionHolder));

        public string Location {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(DeviceDescriptionHolder));

        public string Name {
            get { return ( string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof( string), typeof(DeviceDescriptionHolder));

		//TODO:place it in synesis specific plugin
		public string DeviceIconUri {
			get { return (string)GetValue(DeviceIconUriProperty); }
			set { SetValue(DeviceIconUriProperty, value); }
		}
		public static readonly DependencyProperty DeviceIconUriProperty =
			DependencyProperty.Register("DeviceIconUri", typeof(string), typeof(DeviceDescriptionHolder));

        public IIdentificationModel model {
            get { return (IIdentificationModel)GetValue(modelProperty); }
            set { SetValue(modelProperty, value);}
        }
        // Using a DependencyProperty as the backing store for model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty modelProperty =
            DependencyProperty.Register("model", typeof(IIdentificationModel), typeof(DeviceDescriptionHolder));

        public INvtSession session {
            get { return (INvtSession)GetValue(sessionProperty); }
            set { SetValue(sessionProperty, value); }
        }
        // Using a DependencyProperty as the backing store for session.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty sessionProperty =
            DependencyProperty.Register("session", typeof(INvtSession), typeof(DeviceDescriptionHolder));
    }

	[XmlRootAttribute(ElementName = "ManualDevice", IsNullable = false)]
	public class ManualDevice {
		public string DevUri { get; set; }
	}
	public class ManualUriManager {
		static string path = AppDefaults.ConfigFolderPath + "manuallist.xml";
		public static void Save(List<ManualDevice> manlist) {
			try {
				if (File.Exists(path))
					File.Delete(path);

				using (var sr = File.CreateText(path)) {
					XmlSerializer serializer = new XmlSerializer(typeof(List<ManualDevice>));

					serializer.Serialize(sr, manlist);
				}
			} catch (Exception err) {
				dbg.Error(err);
			}
		}
		public static List<ManualDevice> Load() {
			if (!File.Exists(path))
				return new List<ManualDevice>();
			try {
				using (var sr = File.OpenText(path)) {
					XmlSerializer deserializer = new XmlSerializer(typeof(List<ManualDevice>));
					List<ManualDevice> manlst;
					manlst = (List<ManualDevice>)deserializer.Deserialize(sr);
					return manlst;
				}
			} catch (Exception err) {
				dbg.Error(err);
				return new List<ManualDevice>();
			}
		}
	}
}
