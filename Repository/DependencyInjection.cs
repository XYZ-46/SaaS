using InterfaceProject.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDIRepository(this IServiceCollection services)
        {
            services.AddTransient<IUserLoginRepository, UserLoginRepository>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();

            return services;
        }
    }
}
