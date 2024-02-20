using InterfaceProject.User;
using Microsoft.Extensions.DependencyInjection;
using Repository.User;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDIRepository(this IServiceCollection services)
        {
            return services
                .AddScoped<IUserLoginRepository, UserLoginRepository>()
                //.AddScoped<IBaseQueryRepository, BaseQueryRepository<>>()
                .AddScoped<IUserProfileRepository, UserProfileRepository>();
        }
    }
}
