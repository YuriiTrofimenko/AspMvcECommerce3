using AspNetMvcECommerce.Domain.EntityController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETMVC_ECommerce_3.Domain
{
    public interface IRepository
    {
        UserEC UserEC { get; }
        RoleEC RoleEC { get; }
        ArticleEC ArticleEC { get; }
        CategoryEC CategoryEC { get; }
    }
}
