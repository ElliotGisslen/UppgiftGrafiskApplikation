using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.NewFolder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApp.MVVM.ViewModels
{
    public partial class OverviewViewModel : ObservableObject
    {

        private readonly IotHubManager _iotHubManager;

        [ObservableProperty]
        ObservableCollection<DeviceItemViewModel> devicesList;



        public OverviewViewModel(IotHubManager iotHubManager)
        {
            _iotHubManager = iotHubManager;
            UpdateDeviceList();
            _iotHubManager.DeviceItemListUpdated += UpdateDeviceList;
        }

        private void UpdateDeviceList()
        {
            DevicesList = new ObservableCollection<DeviceItemViewModel>(_iotHubManager.DeviceItemList
                .Select(deviceItem => new DeviceItemViewModel(deviceItem)).ToList());
        }

    }

}
