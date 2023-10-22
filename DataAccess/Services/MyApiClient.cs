//using DataAccess.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataAccess.Services
//{
//    public class MyApiClient
//    {
//        private readonly HttpClient _httpClient;

//        public MyApiClient()
//        {
//            _httpClient = new HttpClient();
//            _httpClient.BaseAddress = new Uri("https://iotdevicefunctions.azurewebsites.net/api/devices?code=MZv5RErPGwp2TqZ5XmclwFQvIONtXQHmsrmT5-udfs7fAzFu0iXOgw==");
//        }

//        public async Task<List<DeviceItem>> GetDataFromApiAsync()
//        {

//            var devices = new List<DeviceItem>();

//            try
//            {
//                HttpResponseMessage response = await _httpClient.GetAsync("/api/devices");

//                if (response.IsSuccessStatusCode)
//                {
//                    string content = await response.Content.ReadAsStringAsync();
//                    foreach(var device in )
//                    return content;
//                }
//                else
//                {
//                    // Handle error
//                    return "Error occurred";
//                }
//            }
//            catch (Exception ex)
//            {
//                // Handle exception
//                return $"Exception: {ex.Message}";
//            }
//            finally
//            {
//                _httpClient.Dispose();
//            }
//        }
//    }
//}
