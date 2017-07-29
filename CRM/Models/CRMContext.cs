using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CRMContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Company> Companies { get; set; }

        public CRMContext() : base("IdentityDb")
        {}


    }
}