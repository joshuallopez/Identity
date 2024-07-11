using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Models
{
    public class AppUserDBContext : IdentityDbContext<AppUser>
    {
        public AppUserDBContext(DbContextOptions<AppUserDBContext> options) : base(options) { }
    }
}
