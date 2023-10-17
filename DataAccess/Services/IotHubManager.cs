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

namespace DataAccess.Services
{
    public class IotHubManager
    {
        private string _connectionString = string.Empty;
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

        public void Initialize(string connectionString = null!)
        {
            try
            {

                _connectionString = connectionString;

                if (!isConfigured)
                {
                    if (!string.IsNullOrEmpty(_connectionString))
                    {
                        _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
                        _serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);
                        isConfigured = true;

                    }


                }

            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }


        public async Task InitializeAsync(string connectionString = null!)
        {
            try
            {
                if(!isConfigured)
                {
                    if(string.IsNullOrEmpty(connectionString))
                    {
                        var settings = await _context.Settings.FirstOrDefaultAsync();
                        if (settings != null)
                        {
                            _registryManager = RegistryManager.CreateFromConnectionString(settings.ConnectionString);
                            _serviceClient = ServiceClient.CreateFromConnectionString(settings.ConnectionString);
                            isConfigured = true;
                        }

                    }
                    else
                    {
                        _registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                        _serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
                        isConfigured = true;

                    }


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

                    foreach (var item in await result.GetNextAsTwinAsync())
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
                        DeviceItemListUpdated!.Invoke();
                            

                }


            }
            catch (Exception ex) { Debug.Write(ex.Message); }
        }

        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            try
            {
                var device = await _registryManager!.GetDeviceAsync(deviceId);
                if (device != null)
                    return device;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }

        public async Task<Device> RegisterDeviceAsync(string deviceId)
        {
            try
            {
                var device = await _registryManager!.AddDeviceAsync(new Device(deviceId));
                if (device != null)
                    return device;
            }
            catch (Exception ex) { Debug.Write(ex.Message); }
            return null!;

        }

        public async Task<bool> InvokeDirectMethodAsync(string deviceId, string methodName, string payload)
        {
            try
            {
                if (isConfigured)
                {
                    var methodInvocation = new CloudToDeviceMethod(methodName)
                    {
                        ResponseTimeout = TimeSpan.FromSeconds(30)
                    };

                    methodInvocation.SetPayloadJson(payload);

                    var response = await _serviceClient!.InvokeDeviceMethodAsync(deviceId, methodInvocation);

                    if (response.Status == 200)
                    {
                        // Direct method invoked successfully
                        var result = response.GetPayloadAsJson();
                        // Handle the result as needed
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error invoking direct method: {ex.Message}");
            }

            return false;
        }







        public string GenerateConnectionString(Device device)
        {
            try
            {
                return $"{_connectionString.Split(";")[0]};Deviceid={device.Id};SharedAccesskey={device.Authentication.SymmetricKey.PrimaryKey}";

            }
            catch (Exception ex) { Debug.Write(ex.Message); }
            return null!;
        }
    }
}
