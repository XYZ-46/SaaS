using Microsoft.EntityFrameworkCore;

namespace Middleware.Database
{
    public class DBAppContext : DbContext
    {
        public DBAppContext(DbContextOptions<DBAppContext> options) : base(options)
        { }
    }
}
