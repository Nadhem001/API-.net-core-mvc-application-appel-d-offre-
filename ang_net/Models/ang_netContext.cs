using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ang_net.Models
{
    public partial class ang_netContext : IdentityDbContext<applicationUser, ApplicationRole,string>
    {
        public ang_netContext(DbContextOptions<ang_netContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Use> Use { get; set; }
       
    }
}
