using LiteDB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SIPVS.litedb
{
    public class LiteDbContext : ILiteDbContext
    {
        public LiteDatabase Database { get; }

        public LiteDbContext(IHostingEnvironment environment, IConfiguration configuration)
        {
            string dbPath = configuration["DATA_REPOSITORY_DB"];
            if (dbPath == null || dbPath.Length == 0)
            {
                dbPath = Path.Combine(
                    environment.ContentRootPath, "data-repository.litedb");
            }
            this.Database = new LiteDatabase(dbPath);

        }
    }
}