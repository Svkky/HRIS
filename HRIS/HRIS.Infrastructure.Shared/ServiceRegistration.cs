using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Settings;
using HRIS.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRIS.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.Configure<MailMessageSettings>(_config.GetSection("MailMessageSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ISubCategoryService, SubSubCategoryService>();
            services.AddTransient<ICustomerVoucherService, CustomerVoucherService>();
            services.AddTransient<IProductTypeVariationService, ProductTypeVariationService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddScoped<IVatService, VatService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IAssignMenuToUserService, AssignMenuToService>();
            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<IBranchAdminService, BranchAdminService>();
            services.AddScoped<IAuditRepository, AuditRepository>();
            services.AddScoped<IStoreProductService, StoreProductService>();
            services.AddScoped<IProductAllocationRepository, ProductAllocationRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        }
    }
}
