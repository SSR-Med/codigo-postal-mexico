namespace Api
{
    using Api.Configurations;
    using Api.Filters;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Core.Dtos.AppSettingsDto;
    using Microsoft.Extensions.Options;
    using Api.Middlewares;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using HealthChecks.UI.Client;
    using FluentValidation;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using Api.Binders;

    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDependencyInjection(Configuration);

            ValidatorOptions.Global.PropertyNameResolver = (type, memberInfo, expression) => Regex.Replace(memberInfo!.Name, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new SnakeModelBinderProvider());
                options.Filters.Add<ApiExceptionFilterAttribute>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
            });
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddDataProtection();
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services.AddSwaggerConfiguration();
            services.AddApiVersioningConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SwaggerSettingDto> swaggerSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }
            app.UseCors("AllowAll");
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<HeaderValidationMiddleware>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecksUI(config =>
            {
                config.UIPath = "/actuator";
            });
            app.AddSwaggerConfiguration(swaggerSettings);
        }
    }
}
