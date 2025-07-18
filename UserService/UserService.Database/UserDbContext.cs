using Microsoft.EntityFrameworkCore;
using UserService.Database.BusinessEntity;

namespace UserService.Database
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<UserData> Users { get; set; }
    }
}
