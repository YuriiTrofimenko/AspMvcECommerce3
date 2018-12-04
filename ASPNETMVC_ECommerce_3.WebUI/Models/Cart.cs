using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVC_ECommerce_3.WebUI.Models
{
    public class Cart
    {
        public string UserName { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}