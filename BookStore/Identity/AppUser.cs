using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using BookStore.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Identity
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}