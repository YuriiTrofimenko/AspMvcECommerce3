﻿using ASPNETMVC_ECommerce_3.Domain;
using ASPNETMVC_ECommerce_3.Domain.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetMvcECommerce.Domain.EntityController
{
    public class RoleEC : AbstractEC<Role>
    {
        public RoleEC(AspNetMvcECommerce_3Entities _db) : base(_db){
        }
    }
}
