namespace Api.Configurations
{
    public static class PoliciesConfiguration
    {
        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrador", policy => policy.RequireRole("administrador"));
            });

            return services;
        }
    }
}