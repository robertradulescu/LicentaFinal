using Microsoft.AspNetCore.Identity;
using LicentaFinal.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LicentaFinal.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {

    }
}
