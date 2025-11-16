namespace Infrastructure
{
    using Core.Interfaces.Services;
    using Infrastructure.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICodigoPostalService, CodigoPostalService>();

            return services;
        }

    }
}