using FRN.Domain._2._2_Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace FRN.Infra._3._1_Context
{
    public class FRNContext : DbContext 
    {
        public FRNContext(DbContextOptions<FRNContext> options) : base(options) { }
        public DbSet<Product> Product { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_BIN");
            modelBuilder.Entity<Product>().HasKey(p => p.cod_produto);
            modelBuilder.Entity<Fornecedor>().HasKey(f => f.cod_fornecedor);
            
        }
    }
}
