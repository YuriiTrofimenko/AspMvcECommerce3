using ASPNETMVC_ECommerce_3.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASPNETMVC_ECommerce_3.WebUI.Controllers
{
    public class DemoController : ApiController
    {
        private IRepository mRepository;
        public DemoController(IRepository _repository)
        {
            mRepository = _repository;
        }

        // GET: api/Demo
        public List<Category> Get()
        {
            //return new string[] { "value1", "value2" };
            return mRepository.CategoryEC.Categories.ToList();
        }

        // GET: api/Demo/5
        public Object Get(int id)
        {
            //return "value";
            //return mRepository.UserEC.Find(id);
            //1
            /*return mRepository.UserEC
                .mDb.Users
                .Where(u => u.id == id)
                .Select(u => new { name = u.login, role = u.Role.name })
                .SingleOrDefault();*/
            //2 (смотри класс сущности Role)
            return mRepository.UserEC.Find(id);
        }

        // POST: api/Demo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Demo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Demo/5
        public void Delete(int id)
        {
        }
    }
}
