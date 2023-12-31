﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class DeviceItem
    {
        public string DeviceId { get; set; } = null!;
        public string DeviceType { get; set; } = null!;
        public string? Manufacturer { get; set; }
        public string? Location { get; set; }
        public bool IsActive { get; set; }
    }
}
