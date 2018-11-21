using ASPNETMVC_ECommerce_3.Domain;
using ASPNETMVC_ECommerce_3.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
                                    login = Uri.UnescapeDataString(_signupForm.Login)
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
                                mRepository.UserEC
                                    .FindByLogin(Uri.UnescapeDataString(_signupForm.Login));

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
                case AuthForm.ActionType.SIGN_OUT:
                    {
                        try
                        {
                            HttpContext.Current.Session["username"] = null;
                            return new ApiResponse()
                            {
                                status = "success"
                                ,message = "logout"
                            };
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
                default:
                    return new ApiResponse() {
                        status = "error"
                        , message = "Unknown action type"
                    };
            }
        }

        [Route("api/auth/checkauth")]
        public ApiResponse Get()
        {
            ApiResponse response = null;
            try
            {
                if (HttpContext.Current.Session["username"] != null)
                {
                    response =
                        new ApiResponse()
                        {
                            status = "success"
                            ,
                            message = "signed"
                            ,
                            data = HttpContext.Current.Session["username"]
                        };
                }
                else
                {
                    response =
                        new ApiResponse()
                        {
                            status = "success"
                            ,
                            message = "unsigned"
                        };
                }
                
            }
            catch (Exception ex)
            {

                response =
                    new ApiResponse()
                    {
                        status = "error"
                                ,
                        message = ex.Message
                    };
            }
            return response;
        }

        [Route("api/auth/page")]
        public Object Get([FromUri] NavigationData _navigationData)
        {
            if (_navigationData.pagename == "admin" || _navigationData.pagename == "adminunit")
            {
                if (HttpContext.Current.Session["username"] != null)
                {

                    User user =
                        mRepository.UserEC.FindByLogin(HttpContext.Current.Session["username"].ToString());
                    if (user.Role.name == "admin")
                    {
                        
                        return GetHTMLPageText(AppDomain.CurrentDomain.BaseDirectory + "\\wwwroot\\pages\\" + _navigationData.pagename + ".htm", _navigationData.param);
                    }
                    else
                    {
                        
                        return GetHTMLErrorPageText("Sign in as admin");
                    }
                }
                else
                {
                    
                    return GetHTMLErrorPageText("Sign in first");
                }
            }
            else
            {
                
                return GetHTMLPageText(AppDomain.CurrentDomain.BaseDirectory + "\\wwwroot\\pages\\" + _navigationData.pagename + ".htm", _navigationData.param);
            }
        }

        //Шифрование строки
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

        //Чтение содержимого текстового файла
        private Object GetHTMLPageText(string _pageUri, string _param)
        {
            var response = new HttpResponseMessage();
            string pageText = "";
            using (StreamReader reader = new StreamReader(_pageUri))
            {
                pageText = reader.ReadToEnd();
                if (_param != null && _param != "")
                {
                    pageText = pageText.Replace("param", _param);
                }
            }
            response.Content = new StringContent(pageText);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        public static Object GetHTMLErrorPageText(string _messageText)
        {
            var response = new HttpResponseMessage();
            string page = "";
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\wwwroot\\pages\\error.htm"))
            {
                page = reader.ReadToEnd();
                page = page.Replace("{{error-text}}", _messageText);
            }
            response.Content = new StringContent(page);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
