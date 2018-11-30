using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVC_ECommerce_3.WebUI.Models
{
    public class CartAction
    {
        public enum ActionType
        {
            neg,
            add,
            rem
        }

        public ActionType actionTypeValue { get; set; }
    }
}