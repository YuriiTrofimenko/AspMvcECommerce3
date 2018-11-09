using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVC_ECommerce_3.WebUI.Models
{
    public class ApiResponse
    {
        public dynamic data { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }
}