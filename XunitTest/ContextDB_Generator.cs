using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace XunitTest
{
    public static class ContextDB_Generator
    {
        public static AzureDB AzureGenerator()
        {
            var optionBuilder = new DbContextOptionsBuilder<AzureDB>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            return new AzureDB(optionBuilder.Options);
        }
    }
}
