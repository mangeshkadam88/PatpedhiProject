using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Patpedhi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Patpedhi.Web
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PatpedhiContext>
    {
        public PatpedhiContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<PatpedhiContext>();

            var connectionString = configuration.GetConnectionString("SQLDBConnectionLocal");

            builder.UseSqlServer(connectionString);

            return new PatpedhiContext(builder.Options);
        }
    }
}
