using InterfaceProject.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDIRepository(this IServiceCollection services)
        {
            return services
                .AddScoped<IUserLoginRepository, UserLoginRepository>()
                .AddScoped<IUserProfileRepository, UserProfileRepository>();
        }
    }
}
