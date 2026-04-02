using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace PE_DAL.Oracle.Context
{
    public class PE_DBContextFactory : IDesignTimeDbContextFactory<PE_DBContext>
    {
        public PE_DBContext CreateDbContext(string[] args)
            => Create();

        public static PE_DBContext Create()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PE_DBContext>();

          
            optionsBuilder.UseOracle( 
                configuration.GetConnectionString("PEDbConnString")
            );

            return new PE_DBContext(optionsBuilder.Options);
        }
    }
}