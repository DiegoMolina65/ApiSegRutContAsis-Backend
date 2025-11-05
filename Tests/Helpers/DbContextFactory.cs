using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Helpers
{
    public static class DbContextFactory
    {
        public static SegRutContAsisContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<SegRutContAsisContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new SegRutContAsisContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
