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
    public class ArticleController : ApiController
    {
        private IRepository mRepository;
        public ArticleController(IRepository _repository)
        {
            mRepository = _repository;
        }

        [Route("api/articles")]
        public Object Get()
        {

            ApiResponse articlesResponse;
            List<Article> articles = null;
            try
            {
                articles =
                    mRepository.ArticleEC.Articles.ToList();

                articlesResponse = new ApiResponse()
                {
                    status = "success",
                    message = "All the articles",
                    data = articles
                };
            }
            catch (Exception ex)
            {
                articlesResponse = new ApiResponse()
                {
                    status = "error",
                    message = "No articles",
                    data = ex.Message
                };
            }

            return articlesResponse;
        }

        [Route("api/articles/get-filtered")]
        public Object Post(FilterForm _filterModel)
        {
            bool filterByCategory =
                (_filterModel != null && _filterModel.categories != null)
                ? true
                : false;

            int[] categoryIds = null;

            var query = mRepository.ArticleEC.Articles;

            if (_filterModel != null)
            {

                if (filterByCategory)
                {
                    categoryIds = _filterModel.categories;
                    query =
                       query.Where(a =>
                       {
                           bool selected = false;
                           foreach (int categoryId in categoryIds)
                           {
                               if (a.category_id == categoryId)
                               {
                                   selected = true;
                                   break;
                               }
                           }
                           return selected;
                       });
                }

                switch (_filterModel.sort)
                {
                    case FilterForm.OrderBy.sortDesc:
                        switch (_filterModel.sortParam)
                        {
                            case FilterForm.SortParam.sortTitle:
                                query = query.OrderByDescending((a => a.title));
                                break;
                            case FilterForm.SortParam.sortCategory:
                                query = query.OrderByDescending((a => a.Category.name));
                                break;
                            case FilterForm.SortParam.sortPrice:
                                query = query.OrderByDescending((a => a.price));
                                break;
                            case FilterForm.SortParam.sortQuantity:
                                query = query.OrderByDescending((a => a.quantity));
                                break;
                            default:
                                break;
                        }
                        break;
                    case FilterForm.OrderBy.sortAsc:
                        switch (_filterModel.sortParam)
                        {
                            case FilterForm.SortParam.sortTitle:
                                query = query.OrderBy((a => a.title));
                                break;
                            case FilterForm.SortParam.sortCategory:
                                query = query.OrderBy((a => a.Category.name));
                                break;
                            case FilterForm.SortParam.sortPrice:
                                query = query.OrderBy((a => a.price));
                                break;
                            case FilterForm.SortParam.sortQuantity:
                                query = query.OrderBy((a => a.quantity));
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            query =
                query.Select(
                        (a => {
                            if (a.image_base64 == null || a.image_base64 == "")
                            {
                                a.image_base64 = "/wwwroot/images/no-image.png";
                            }
                            return a;
                        })
                    );

            return new ApiResponse()
            {
                status = "success",
                message = "Filtered articles",
                data = query.ToList()
            };
        }

        [Route("api/articles/add")]
        public ApiResponse Post([FromBody]ArticleForm _articleForm)
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
                            mRepository.CategoryEC.Find(_articleForm.Categoryid);
                        Article article = new Article()
                        {
                            title = Uri.UnescapeDataString(_articleForm.Title)
                            ,
                            category_id = _articleForm.Categoryid
                            ,
                            description = Uri.UnescapeDataString(_articleForm.Description)
                            ,
                            price = _articleForm.Price
                            ,
                            quantity = _articleForm.Quantity
                            ,
                            Category = category
                            ,
                            image_base64 = (_articleForm.ImageBase64 ?? "")
                            ,
                            Article_details =
                                new List<Article_details>() { }
                            ,
                            image_url = ""

                        };
                        mRepository.ArticleEC.Save(article);
                        return new ApiResponse()
                        {
                            status = "success",
                            message = $"Article {article.title} created",
                            data = article
                        };
                    }
                    else
                    {
                        return new ApiResponse()
                        {
                            status = "error",
                            message = "Article wasn't created",
                            data = "Sign in as admin"
                        };
                    }
                }
                else
                {
                    return new ApiResponse()
                    {
                        status = "error",
                        message = "Article wasn't created",
                        data = "Sign in first"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    status = "error",
                    message = "Article wasn't created",
                    data = ex.Message
                };
            }
        }

        // POST: api/Article
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Article/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [Route("api/articles/delete/{id}")]
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
                        Article article = mRepository.ArticleEC.Find(id);
                        mRepository.ArticleEC.Remove(article);
                        return new ApiResponse()
                        {
                            status = "success",
                            data = article,
                            message = $"Article {article.title} deleted"
                        };
                    }
                    else
                    {
                        return new ApiResponse()
                        {
                            status = "error",
                            message = "Article wasn't created",
                            data = "Sign in as admin"
                        };
                    }
                }
                else
                {
                    return new ApiResponse()
                    {
                        status = "error",
                        message = "Article wasn't created",
                        data = "Sign in first"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    status = "error",
                    message = "Article wasn't deleted",
                    data = ex.Message
                };
            }
        }
    }
}
