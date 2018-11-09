using ASPNETMVC_ECommerce_3.Domain;
using ASPNETMVC_ECommerce_3.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ASPNETMVC_ECommerce_3.WebUI.Controllers
{
    public class AuthController : ApiController
    {
        public static int CUSTOMER_ROLE_ID = 2;
        public static int ADMIN_ROLE_ID = 1;

        private IRepository mRepository;
        public AuthController(IRepository _repository)
        {
            mRepository = _repository;
        }

        [Route("api/auth")]
        public ApiResponse Post([FromBody] AuthForm _signupForm)
        {
            switch (_signupForm.Action)
            {
                case AuthForm.ActionType.SIGN_UP:
                    {
                        try
                        {
                            string newUserName = _signupForm.Login;
                            var oldUser =
                                mRepository.UserEC.FindByLogin(newUserName);
                            if (oldUser == null)
                            {
                                Role role =
                                    mRepository.RoleEC.Find(CUSTOMER_ROLE_ID);
                                User user = new User()
                                {
                                    login = _signupForm.Login
                                    ,
                                    password = StringToMD5(_signupForm.Password)
                                    ,
                                    Role = role
                                    ,
                                    role_id = role.id
                                };
                                mRepository.UserEC.Save(user);
                                return new ApiResponse() {
                                    status = "success"
                                    , message = $"User {newUserName} created"
                                };
                            }
                            else
                            {
                                return new ApiResponse() {
                                    status = "error"
                                    , message = $"User {newUserName} already exists"
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            return new ApiResponse() {
                                status = "error"
                                , message = ex.Message
                            };
                        }
                    }
                case AuthForm.ActionType.SIGN_IN:
                    {
                        try
                        {
                            User user =
                                mRepository.UserEC.FindByLogin(_signupForm.Login);

                            if (user != null && StringToMD5(_signupForm.Password) == user.password)
                            {

                                HttpContext.Current.Session["username"] = _signupForm.Login;
                                return new ApiResponse() {
                                    status = "success"
                                    ,
                                    message = "signed"
                                    ,
                                    data = _signupForm.Login
                                };
                            }
                            else
                            {

                                return new ApiResponse() {
                                    status = "error"
                                    ,
                                    message = $"User {_signupForm.Login} not found or password is incorrect"
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            return new ApiResponse()
                            {
                                status = "error"
                                ,
                                message = ex.Message
                            };
                        }
                    }
                /*case HttpRequestParams.signout:
                    {
                        try
                        {
                            HttpContext.Current.Session["username"] = null;
                            return new ApiResponse() { data = new List<string>() { "logout" }, error = "" };
                        }
                        catch (Exception ex)
                        {

                            return new ApiResponse() { data = null, error = ex.Message };
                        }
                    }*/
                default:
                    return new ApiResponse() {
                        status = "error"
                        , message = "Unknown action type"
                    };
            }
        }

        static String StringToMD5(String _string)
        {
            byte[] hash = Encoding.UTF8.GetBytes(_string);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";
            foreach (var b in hashenc)
            {
                result += b.ToString("x2");
            }
            return result;
        }
    }
}
