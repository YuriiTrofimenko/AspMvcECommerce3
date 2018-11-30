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
    public class CartController : ApiController
    {
        private IRepository mRepository;
        public CartController(IRepository _repository)
        {
            mRepository = _repository;
        }

        [Route("api/cart")]
        public Object Get()
        {
            try
            {
                if (HttpContext.Current.Session["username"] != null)
                {
                    if (HttpContext.Current.Session["cart"] == null)
                    {
                        HttpContext.Current.Session["cart"] =
                            new Cart() { CartItems = new List<CartItem>() };
                    }

                    List<CartItemDetails> cartItemDetails =
                        (HttpContext.Current.Session["cart"] as Cart).CartItems
                        .Select(cartItem => {
                            Article article = mRepository.ArticleEC.Find(cartItem.ArticleId);

                            return new CartItemDetails()
                            {
                                ArticleId = cartItem.ArticleId
                                ,
                                ArticleName = article.title
                                ,
                                Count = cartItem.Count
                            };
                        })
                        .ToList();

                    return new ApiResponse()
                    {
                        status = "success",
                        data = cartItemDetails
                    };
                }
                else
                {
                    return new ApiResponse() {
                        status = "error",
                        message = "Sign in first"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    status = "error",
                    message = ex.Message + " : " + ex.StackTrace
                };
            }
        }

        [Route("api/cart/{artid}")]
        public Object Post([FromUri] string artid, [FromBody] CartAction cartAction)
        {
            try
            {
                int artidInt = Int32.Parse(artid);
                if (HttpContext.Current.Session["username"] != null)
                {
                    if (HttpContext.Current.Session["cart"] == null)
                    {
                        HttpContext.Current.Session["cart"] =
                            new Cart() { CartItems = new List<CartItem>() };
                    }

                    Cart cart = (Cart)HttpContext.Current.Session["cart"];
                    CartItem currentCartItem =
                        cart.CartItems.Find(cartItem => cartItem.ArticleId == artidInt);
                    if (currentCartItem == null)
                    {
                        cart.CartItems.Add(new CartItem() { ArticleId = artidInt, Count = 0 });
                        currentCartItem =
                            cart.CartItems.Find(cartItem => cartItem.ArticleId == artidInt);
                    }
                    if (cartAction.actionTypeValue == CartAction.ActionType.neg)
                    {
                        currentCartItem.Count--;
                        if (currentCartItem.Count <= 0)
                        {
                            cart.CartItems.Remove(currentCartItem);
                        }
                    }
                    else if (cartAction.actionTypeValue == CartAction.ActionType.rem)
                    {
                        cart.CartItems.Remove(currentCartItem);
                    }
                    else if (cartAction.actionTypeValue == CartAction.ActionType.add)
                    {
                        currentCartItem.Count++;
                    }


                    HttpContext.Current.Session["cart"] = cart;

                    return new ApiResponse()
                    {
                        status = "success",
                        data =
                        new List<Cart>() {
                            HttpContext.Current.Session["cart"] as Cart
                        }
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        status = "error",
                        message = "Sign in first"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    status = "error",
                    message = ex.Message + " : " + ex.StackTrace
                };
            }
        }
    }
}
