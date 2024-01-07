using DataEntity.User;
using Microsoft.EntityFrameworkCore;


namespace Repository.Database
{
    public class AzureDB(DbContextOptions<AzureDB> options) : DbContext(options)
    {
        public DbSet<UserLoginModel> UserLoginModel { get; set; }
        public DbSet<UserProfileModel> UserProfileModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
