using FastKart.Constans;
using FastKart.DAL;
using FastKart.DAL.Entities;
using FastKart.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("Default"), sqlServerOptions => sqlServerOptions.MigrationsAssembly(nameof(FastKart))));

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
        });

        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

  
        FileConstans.SliderImagePath = Path.Combine(builder.Environment.WebRootPath, "assets", "images", "fashion", "home-banner");

        builder.Services.Configure<SuperAdmin>(builder.Configuration.GetSection("SuperAdmin"));
       
        var app = builder.Build();

        // ????????????? ??????
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var superAdmin = scope.ServiceProvider.GetService<IOptions<SuperAdmin>>();
            var dataInitializer = new DataInitializer(userManager, roleManager, appDbContext, superAdmin);
            await dataInitializer.SeedDataAsync();
        }

        // ????????? middleware
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSession();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // ????????? ?????????
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
            );
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
        });

        // ?????? ??????????
        await app.RunAsync();
    }
}
