using System;
using Microsoft.AspNetCore.Identity;

namespace WebBanHang.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { set; get; }
        public DateTime BirthDay { set; get; }
    }
}
