using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace C_Means.Data
{
    public partial class DB_Context : DbContext
    {
        public DB_Context()
            : base("name=DB_Context")
        {
        }

        public virtual DbSet<Iris> Irides { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Iris>()
                .Property(e => e.Sepal_Length)
                .IsUnicode(false);

            modelBuilder.Entity<Iris>()
                .Property(e => e.Sepal_Width)
                .IsUnicode(false);

            modelBuilder.Entity<Iris>()
                .Property(e => e.Petal_Length)
                .IsUnicode(false);

            modelBuilder.Entity<Iris>()
                .Property(e => e.Petal_Width)
                .IsUnicode(false);

            modelBuilder.Entity<Iris>()
                .Property(e => e.Class)
                .IsUnicode(false);
        }
    }
}
