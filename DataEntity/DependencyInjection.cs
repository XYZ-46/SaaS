using DataEntity.Request;
using DataEntity.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataEntity
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDIEntity(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();

            return services;
        }
    }
}
