using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity.Database
{
    public class AzureDB : DbContext
    {
        public AzureDB(DbContextOptions<AzureDB> options) : base(options)
        { }
    }
}
