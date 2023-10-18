using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApp.MVVM.Models
{
    public class WeatherData
    {
        public string ApprovedTime { get; set; }
        public string ReferenceTime { get; set; }
        public Geometry Geometry { get; set; }
        public List<TimeSeries> TimeSeries { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }
        public List<List<double>> Coordinates { get; set; }
    }

    public class TimeSeries
    {
        public string ValidTime { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string LevelType { get; set; }
        public int Level { get; set; }
        public string Unit { get; set; }
        public List<double> Values { get; set; }
    }
}
