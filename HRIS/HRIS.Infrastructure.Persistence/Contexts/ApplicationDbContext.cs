using HRIS.Application.Interfaces;
using HRIS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public virtual DbSet<BranchProduct> BranchProducts { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerReturn> CustomerReturn { get; set; }
        public virtual DbSet<CustomerVoucher> CustomerVoucher { get; set; }
        public virtual DbSet<ProductTypeVariation> ProductTypeVariation { get; set; }
        public virtual DbSet<Expenditure> Expenditure { get; set; }
        public virtual DbSet<StorePurchase> StorePurchase { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<Sales> Sales { get; set; }
        public virtual DbSet<StoreSetup> StoreSetup { get; set; }
        public virtual DbSet<VAT> VAT { get; set; }
        public virtual DbSet<ReceiptNoGenerator> ReceiptNoGenerator { get; set; }
        public virtual DbSet<VendorReturn> VendorReturn { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }
        public virtual DbSet<MenuSetup> MenuSetup { get; set; }
        public virtual DbSet<UsersRolePermission> UsersRolePermission { get; set; }
        public virtual DbSet<BranchAdmin> BranchAdmin { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<SalesDetail> SalesDetail { get; set; }
        public virtual DbSet<Audit> Audit { get; set; }
        public virtual DbSet<StoreProduct> StoreProduct { get; set; }
        public virtual DbSet<ProductAllocation> ProductAllocation { get; set; }
        public virtual DbSet<ProductAllocationUse> ProductAllocationUse { get; set; }
        public virtual DbSet<VendorPaymentMaster> VendorPaymentMaster { get; set; }
        public virtual DbSet<ProductWareHouse> ProductWareHouse { get; set; }



        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedOn = _dateTime.NowUtc;
                        entry.Entity.UpdatedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }
            base.OnModelCreating(builder);

        }
    }
}
