using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Models;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApp.MVVM.ViewModels
{
    public partial class DeviceItemViewModel : ObservableObject
    {
        private DeviceItem _deviceItem;

        public DeviceItemViewModel(DeviceItem deviceItem)
        {
            _deviceItem = deviceItem;
            Location = deviceItem.Location ?? "";
            IsActive = deviceItem.IsActive;
            Icon = SetDeviceIcon();
        }


        public string DeviceId => _deviceItem.DeviceId ?? "";
        public string DeviceType => _deviceItem.DeviceType ?? "";
        public string Manufacturer => _deviceItem.Manufacturer ?? "";

        private string _location;
        private bool _isActive;
        private string _icon;

        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }




        private string SetDeviceIcon()
        {
            return DeviceType.ToLower() switch
            {
                "light" => "\uf0eb",
                _ => "\uf2db",
            };
        }
    }
}
