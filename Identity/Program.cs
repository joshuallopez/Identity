using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppUserDBContext>(options =>
                options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
            builder.Services.AddIdentity<AppUser,IdentityRole>().
                AddEntityFrameworkStores<AppUserDBContext>().AddDefaultTokenProviders();

           // builder.Services.ConfigureApplicationCookie(opt => opt.LoginPath = "/Authenticate/Login");
            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
