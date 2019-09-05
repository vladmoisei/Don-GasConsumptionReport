using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public class RaportareDbContext : DbContext
    {
        public RaportareDbContext(DbContextOptions<RaportareDbContext> options)
            : base(options)
        { }

        public DbSet<IndexModel> IndexModels { get; set; }
        public DbSet<ConsumGazModel> ConsumGazModels{ get; set; }
    }
}
