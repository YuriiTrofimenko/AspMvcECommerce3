using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVC_ECommerce_3.WebUI.Models
{
    public class AuthForm
    {
        public static class ActionType {
            public const String SIGN_IN = "SIGN_IN";
            public const String SIGN_UP = "SIGN_UP";
            public const String SIGN_OUT = "SIGN_OUT";
        }
        public string Login { get; set; }
        public string Password { get; set; }
        public String Action { get; set; }
    }
}