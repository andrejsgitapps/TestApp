using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Api.Dal.Models;

namespace TestApp.Api.Dal
{
    public class TestAppDbContext : DbContext
    {
        public DbSet<LookupWord> LookupWords { get; set; }
        public DbSet<SearchString> SearchStrings { get; set; }

        public TestAppDbContext(DbContextOptions<TestAppDbContext> options) : base(options)
        {
        }
    }
}
