
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK_Kafka.DBContext.Modelos;

namespace ZK_Kafka.DBContext
{
    class GenHorasEntitidades: DbContext
    {
        private readonly string _connectionString;

        public GenHorasEntitidades(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<SPBioData> SPBioData { get; set; }

    }
}
