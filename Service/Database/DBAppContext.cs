using Microsoft.EntityFrameworkCore;

namespace Service.Database
{
    public class DBAppContext : DbContext
    {
        public DBAppContext(DbContextOptions<DBAppContext> options) : base(options)
        { }
    }
}
