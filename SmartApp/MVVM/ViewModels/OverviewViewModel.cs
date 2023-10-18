using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Contexts;
using DataAccess.Models;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceApplication.Services;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace SmartApp.MVVM.ViewModels
{
    public partial class OverviewViewModel : ObservableObject
    {

        private readonly IotHubManager _iotHubManager;
        private readonly DateTimeService _dateTimeService;

        [ObservableProperty]
        ObservableCollection<DeviceItemViewModel> devicesList;

        [ObservableProperty]
        string result;

        [ObservableProperty]
        string currentTime;

        [ObservableProperty]
        string currentDate;


        public OverviewViewModel(IotHubManager iotHubManager, DateTimeService dateTimeService)
        {
            _iotHubManager = iotHubManager;
            _dateTimeService = dateTimeService;
            _iotHubManager.InitializeAsync().ConfigureAwait(true);
            UpdateDeviceList();
            _iotHubManager.DeviceItemListUpdated += UpdateDeviceList;
            Task.Run(() => GetDateTime());

        }


        private void GetDateTime()
        {
            while (true)
            {
                CurrentTime = _dateTimeService.CurrentTime;

                CurrentDate = _dateTimeService.CurrentDate;
            }
        }



        public void CheckIfListIsEmpty()
        {
            if(DevicesList.Count == 0)
            {
                Result = "Loading Devices...";
            }
            else
            {
                Result = "All Devices";
            }

        }



        private void UpdateDeviceList()
        {
            DevicesList = new ObservableCollection<DeviceItemViewModel>(_iotHubManager.DeviceItemList
                .Select(deviceItem => new DeviceItemViewModel(deviceItem)).ToList());
            CheckIfListIsEmpty();
        }


        public ICommand ToggleDeviceCommand => new RelayCommand<DeviceItemViewModel>(async (deviceViewModel) => await ToggleDevice(deviceViewModel));
        async Task ToggleDevice(DeviceItemViewModel deviceViewModel)
        {
            if (deviceViewModel != null)
            {
                if (deviceViewModel.IsActive)
                {
                    // Call the StopDevice method
                    await StopDevice(deviceViewModel);
                }
                else
                {
                    // Call the StopDevice method
                    await StartDevice(deviceViewModel);
                }
            }
        }


        async Task StartDevice(DeviceItemViewModel deviceViewModel)
        {
            string deviceId = deviceViewModel.DeviceId;
            await _iotHubManager.InvokeDirectMethodAsync(deviceId, "start", "{\"key\":\"value\"}");

            // Update the IsActive state
            deviceViewModel.IsActive = true;
            
        }

        async Task StopDevice(DeviceItemViewModel deviceViewModel)
        {
            string deviceId = deviceViewModel.DeviceId;
            await _iotHubManager.InvokeDirectMethodAsync(deviceId, "stop", "{\"key\":\"value\"}");

            // Update the IsActive state
            deviceViewModel.IsActive = false;
        }

    }

}
