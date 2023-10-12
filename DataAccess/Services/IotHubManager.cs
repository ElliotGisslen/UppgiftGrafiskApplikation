using DataAccess.Contexts;
using DataAccess.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.NewFolder
{
    public class IotHubManager
    {
        private System.Timers.Timer timer;
        private bool isConfigured;
        private readonly SmartAppDbContext _context;
        private RegistryManager? _registryManager;
        private ServiceClient? _serviceClient;
        public IotHubManager(SmartAppDbContext context)
        {
            _context = context;
            DeviceItemList = new List<DeviceItem>();

            timer = new System.Timers.Timer(5000);
            timer.Elapsed += async (s, e) => await GetAllDevicesAsync();
            timer.Start();
        }

        public List<DeviceItem> DeviceItemList { get; private set; }
        public event Action? DeviceItemListUpdated;


        public async Task InitializeAsync()
        {
            try
            {
                if(!isConfigured)
                {
                    await Task.Delay(100);
                    var connectionString = "HostName=elliot-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=HVjAs0+TdO15cXjtpQj4dZobiOVJaWIxDAIoTHqyU50=";
                    _registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                    _serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

                    //var settings = await _context.Settings.FirstOrDefaultAsync();
                    //if (settings != null)
                    //{
                    //    _registryManager = RegistryManager.CreateFromConnectionString(settings.ConnectionString);
                    //    _serviceClient = ServiceClient.CreateFromConnectionString(settings.ConnectionString);
                    //isConfigured = true;
                    //}
                }

            } catch (Exception ex) { Debug.WriteLine(ex.Message); }

        }

        private async Task GetAllDevicesAsync()
        {
            try
            {
                if (isConfigured)
                {
                    var list_updated = false;
                    var twins = new List<Twin>();
                    var result = _registryManager!.CreateQuery("select * from devices");

                    foreach (var item in await result!.GetNextAsTwinAsync())
                        twins.Add(item);

                    foreach (var device in twins)
                        if (!DeviceItemList.Any(x => x.DeviceId == device.DeviceId))
                        {
                            DeviceItemList.Add(new DeviceItem { DeviceId = device.DeviceId });
                            list_updated = true;
                        }

                    for(int i = DeviceItemList.Count - 1; i >= 0; i--)
                    {
                        if(!twins.Any(x => x.DeviceId == DeviceItemList[i].DeviceId))
                        {
                            DeviceItemList.RemoveAt(i);
                            list_updated = true;
                        }
                    }

                    if (list_updated)
                        DeviceItemListUpdated?.Invoke();
                            

                }


            }
            catch (Exception ex) { Debug.Write(ex.Message); }
        }
    }
}
