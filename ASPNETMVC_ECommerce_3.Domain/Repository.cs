using AspNetMvcECommerce.Domain.EntityController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETMVC_ECommerce_3.Domain
{
    public class Repository : IRepository
    {
        private AspNetMvcECommerce_3Entities mDb;
        public UserEC UserEC => new UserEC(mDb);
        public RoleEC RoleEC => new RoleEC(mDb);
        public CategoryEC CategoryEC => new CategoryEC(mDb);
        public ArticleEC ArticleEC => new ArticleEC(mDb);

        public Repository()
        {
            mDb = new AspNetMvcECommerce_3Entities();
        }
    }
}
