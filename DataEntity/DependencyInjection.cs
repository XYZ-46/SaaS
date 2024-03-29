﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DataEntity
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDIEntity(this IServiceCollection services)
        {
            return services
                .AddFluentValidationAutoValidation()
                //.AddFluentValidationAutoValidation(fv => fv.DisableDataAnnotationsValidation = true)
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
