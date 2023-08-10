using AbysterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AbysterApi.Data
{
    public class AbysterDbContext : DbContext
    {
        public AbysterDbContext()
        {
        }

        public AbysterDbContext(DbContextOptions<AbysterDbContext> options) : base(options)
        {
        }

        public DbSet<Personne> Personnes { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Operation> Operations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Operation>()
                .Property(o => o.Montant)
                .HasColumnType("decimal(18,2)"); 
        }
    }
}
