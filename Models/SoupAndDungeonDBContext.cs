using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SoupAndDungeon.Models
{
    public partial class SoupAndDungeonDBContext : DbContext
    {
        public SoupAndDungeonDBContext()
            : base("name=SoupAndDungeonDBContext")
        {
        }

        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Players> Players { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
