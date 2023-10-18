using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace SmartApp.Services
{
    public class WeatherService
    {
        private readonly string _outsideUrl = "https://api.openweathermap.org/data/2.5/weather?lat=59.1875&lon=18.1232&APPID=5b3e46fcebe5ca46b789a603b30c991f";
        private readonly System.Timers.Timer _timer;
        private readonly HttpClient _http;

        public string? CurrentWeatherCondition { get; private set; }
        public string? CurrentOutsideTemperature { get; private set; }
        public string? CurrentInsideTemperature { get; private set; }
        public event Action? WeatherUpdated;


        public WeatherService(HttpClient http)
        {
            _http = http;
            Task.Run(SetCurrentWeatherAsync);

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += async (s, e) => await SetCurrentWeatherAsync();
            _timer.Start();
        }

        private async Task SetCurrentWeatherAsync()
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(await _http.GetStringAsync(_outsideUrl));
                CurrentOutsideTemperature = (data!.main.temp - 273.15).ToString("#");
                CurrentWeatherCondition = GetWeatherConditionIcon(data!.weather[0].description.ToString());

            }
            catch
            {
                CurrentOutsideTemperature = "--";
                CurrentWeatherCondition = GetWeatherConditionIcon("--");
            }


            WeatherUpdated?.Invoke();
        }

        private string GetWeatherConditionIcon(string value)
        {
            return value switch
            {
                "clear sky" => "\ue28f",
                "few clouds" => "\uf6c4",
                "overcast clouds" => "\uf744",
                "scattered clouds" => "\uf0c2",
                "broken clouds" => "\uf744",
                "shower rain" => "\uf738",
                "rain" => "\uf740",
                "thunderstorm" => "\uf76c",
                "snow" => "\uf742",
                "mist" => "\uf74e",
                _ => "\ue137",
            };
        }


    }
}
