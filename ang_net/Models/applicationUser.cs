using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ang_net.Models
{
    public class applicationUser: IdentityUser
    {
        public string Country { get; set; }
    }
}
