using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.DTW.Infrastructure.ServiceLayer
{
    public class ServiceLayerConfig
    {
        public string BaseUrl { get; set; } = string.Empty; // e.g. https://server:50000
        public string CompanyDb { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
