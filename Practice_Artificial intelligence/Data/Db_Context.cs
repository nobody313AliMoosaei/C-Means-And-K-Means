using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Practice_Artificial_intelligence.Data
{
    public partial class Db_Context : DbContext
    {
        public Db_Context()
            : base("name=Model1")
        {
        }

        public virtual DbSet<ClusterTB> Clusters { get; set; }
        public virtual DbSet<Iris> IrisTb { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClusterTB>()
                .Property(e => e.ClusterName)
                .IsUnicode(false);

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

            modelBuilder.Entity<Iris>()
                .HasMany(e => e.Clusters)
                .WithOptional(e => e.Iris)
                .HasForeignKey(e => e.Point_Id);
        }
    }
}
