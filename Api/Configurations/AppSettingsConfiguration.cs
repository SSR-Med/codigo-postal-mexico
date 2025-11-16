namespace Api.Configurations
{
    using Core.Constants;
    using Core.Dtos.AppSettingsDto;

    public static class AppSettingsConfiguration
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SwaggerSettingDto>(o => configuration.GetSection(AppSettingConstant.Swagger).Bind(o));
            services.Configure<CodigoPostalSettingDto>(o => configuration.GetSection(AppSettingConstant.CodigoPostal).Bind(o));

            return services;
        }
    }
}