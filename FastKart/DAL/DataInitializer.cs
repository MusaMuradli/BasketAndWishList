using FastKart.DAL.Entities;
using FastKart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FastKart.DAL
{
    public class DataInitializer
    {
        private readonly UserManager<AppUser> _userManager ;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        private readonly SuperAdmin _superAdmin;

        public DataInitializer(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext, IOptions<SuperAdmin> superAdmin)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
            _superAdmin = superAdmin.Value;
        }
        public async Task SeedDataAsync()
        {
            await _appDbContext.Database.MigrateAsync();

            var roles = new List<string>() { RoleConstans.Admin, RoleConstans.Moderator, RoleConstans.User };
            foreach (var role in roles)
            {
                if ( await _roleManager.FindByNameAsync(role) != null) continue;
                await _roleManager.CreateAsync(new IdentityRole { Name=role});
            }
           
            var user = new AppUser
            {
                Fullname = _superAdmin.FullName,
                UserName= _superAdmin.UserName,
                Email = _superAdmin.Email,
            };

            if (await _userManager.FindByNameAsync(user.UserName) != null) return;

            var result = await _userManager.CreateAsync(user, _superAdmin.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleConstans.User);
            }
        }
    }
}
