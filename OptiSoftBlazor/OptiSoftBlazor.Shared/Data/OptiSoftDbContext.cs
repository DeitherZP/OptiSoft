using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;
using OptiSoftBlazor.Shared.Data.Aditionals;
using OptiSoftBlazor.Shared.Data.RolePermission;
using OptiSoftBlazor.Shared.Data.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Data
{
    public class OptiSoftDbContext : IdentityDbContext<ApplicationUser>
    {
        public OptiSoftDbContext(DbContextOptions<OptiSoftDbContext> options) : base(options)
        {
        }

        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public DbSet<DetCompra> DetCompra { get; set; }
        public DbSet<Consulta> Consulta { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Seteo> Seteo { get; set; }
        public DbSet<RolSucursal> RolSucursal { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<AppScreen> AppScreen { get; set; }
        public DbSet<RoleScreenPermission> RoleScreenPermission { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }

        public class BlankTriggerAddingConvention : IModelFinalizingConvention
        {
            public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
            {
                foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
                {
                    var table = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
                    if (table != null && NoTriggers(entityType, table.Value) && IsNotTphMapping(entityType))
                    {
                        entityType.Builder.HasTrigger(table.Value.Name + "_Trigger");
                    }

                    foreach (var fragment in entityType.GetMappingFragments(StoreObjectType.Table))
                    {
                        if (NoTriggers(entityType, fragment.StoreObject))
                        {
                            entityType.Builder.HasTrigger(fragment.StoreObject.Name + "_Trigger");
                        }
                    }
                }
            }

            private bool NoTriggers(IConventionEntityType entityType, StoreObjectIdentifier table) =>
                entityType.GetDeclaredTriggers().All(t => t.GetDatabaseName(table) == null);

            private bool IsNotTphMapping(IConventionEntityType entityType) =>
                entityType.BaseType == null || entityType.GetMappingStrategy() != RelationalAnnotationNames.TphMappingStrategy;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Articulo>().ToTable("Articulo", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Cliente>().ToTable("Cliente", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Compra>().ToTable("Compra", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<DetCompra>().ToTable("DetCompra", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Consulta>().ToTable("Consulta", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Personal>().ToTable("Personal", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Seteo>().ToTable("Seteo", t => t.ExcludeFromMigrations());
            //modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser", t => t.ExcludeFromMigrations());

            //modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            modelBuilder.Entity<Consulta>(b =>
            {
                b.ToTable("Consulta", "optica");
                    
                b.HasOne(c => c.Profesional)
                    .WithMany()
                    .HasForeignKey(c => c.IdProfesional)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
