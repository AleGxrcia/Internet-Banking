using InternetBanking.Core.Domain.Common;
using InternetBanking.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace InternetBanking.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //FLUENT API

            #region tables
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Beneficiary>().ToTable("Beneficiaries");
            modelBuilder.Entity<Payment>().ToTable("Payments");
            #endregion

            #region "primary keys"
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Beneficiary>().HasKey(p => p.Id);
            modelBuilder.Entity<Payment>().HasKey(p => p.Id);
            #endregion

            #region relationships
            #endregion

            #region "property configurations"

            #region products
            modelBuilder.Entity<Product>()
                .Property(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.AccountNumber)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Balance)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductTypeId)
                .IsRequired();
            #endregion

            #region beneficiaries
            modelBuilder.Entity<Beneficiary>()
                .Property(p => p.UserOwnerId)
                .IsRequired();
            
            modelBuilder.Entity<Beneficiary>()
                .Property(p => p.AccountNumberBeneficiary)
                .IsRequired();
            #endregion

            #region payments
            modelBuilder.Entity<Payment>()
                .Property(p => p.SourceAccountNumber)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .Property(p => p.DestinationAccountNumber)
                .IsRequired();
            
            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentTypeId)
                .IsRequired();
            #endregion

            #region
            #endregion

            #region
            #endregion


            #endregion
        }
    }
}
