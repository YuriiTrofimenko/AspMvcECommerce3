using ASPNETMVC_ECommerce_3.Domain;
using ASPNETMVC_ECommerce_3.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ASPNETMVC_ECommerce_3.WebUI.Controllers
{
    public class CategoryController : ApiController
    {
        private IRepository mRepository;
        public CategoryController(IRepository _repository)
        {
            mRepository = _repository;
        }

        [Route("api/categories")]
        public ApiResponse Get()
        {
            ApiResponse categoriesResponse;
            List<Category> categories = null;
            try
            {
                categories =
                    mRepository.CategoryEC.Categories.ToList();

                categoriesResponse = new ApiResponse()
                {
                    status = "success",
                    message = "All the categories",
                    data = categories
                };
            }
            catch (Exception ex)
            {
                categoriesResponse = new ApiResponse()
                {
                    status = "error",
                    message = "No categories",
                    data = ex.Message
                };
            }

            return categoriesResponse;
        }

        [Route("api/categories/add")]
        public ApiResponse Post([FromBody]CategoryForm categoryForm)
        {
            try
            {
                if (HttpContext.Current.Session["username"] != null)
                {
                    User user =
                        mRepository.UserEC.FindByLogin(
                                HttpContext.Current.Session["username"].ToString()
                            );
                    if (user.Role.name == "admin")
                    {
                        Category category =
                            new Category() {
                                name = categoryForm.name,
                                Articles = new List<Article>()
                            };
                        mRepository.CategoryEC.Save(category);
                        return new ApiResponse() {
                            status = "success",
                            message = $"Category {category.name} created",
                            data = category
                        };
                    }
                    else
                    {
                        return new ApiResponse()
                        {
                            status = "error",
                            message = "Category wasn't created",
                            data = "Sign in as admin"
                        };
                    }
                }
                else
                {
                    return new ApiResponse()
                    {
                        status = "error",
                        message = "Category wasn't created",
                        data = "Sign in first"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    status = "error",
                    message = "Category wasn't created",
                    data = ex.Message
                };
            }
        }

        // PUT: api/Category/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [Route("api/categories/delete/{id}")]
        public ApiResponse Delete([FromUri]int id)
        {
            try
            {
                if (HttpContext.Current.Session["username"] != null)
                {
                    User user =
                        mRepository.UserEC.FindByLogin(
                                HttpContext.Current.Session["username"].ToString()
                            );
                    if (user.Role.name == "admin")
                    {
                        Category category = mRepository.CategoryEC.Find(id);
                        mRepository.CategoryEC.Remove(category);
                        return new ApiResponse() {
                            status = "success",
                            data = category,
                            message = $"Category {category.name} deleted"
                        };
                    }
                    else
                    {
                        return new ApiResponse()
                        {
                            status = "error",
                            message = "Category wasn't created",
                            data = "Sign in as admin"
                        };
                    }
                }
                else
                {
                    return new ApiResponse()
                    {
                        status = "error",
                        message = "Category wasn't created",
                        data = "Sign in first"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    status = "error",
                    message = "Category wasn't deleted",
                    data = ex.Message
                };
            }
        }
    }
}
