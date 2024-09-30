using Microsoft.AspNetCore.Identity;

namespace FastKart.DAL.Entities
{
    public class AppUser: IdentityUser
    {
        public string Fullname { get; set; }
    }
}
