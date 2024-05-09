using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InternetBanking.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IBeneficiaryService, BeneficiaryService>();
            services.AddTransient<IPaymentService, PaymentService>();
            #endregion
        }
    }
}
