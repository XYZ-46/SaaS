using DataEntity.User;
using Microsoft.EntityFrameworkCore;


namespace Repository.Database
{
    public class AzureDB : DbContext
    {
        public AzureDB(DbContextOptions<AzureDB> options) : base(options)
        { }

        public DbSet<UserLoginModel> UserLoginModel { get; set; }
        public DbSet<UserProfileModel> UserProfileModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
