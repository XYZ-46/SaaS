using DataEntity.Model;

namespace InterfaceProject.Repository
{
    public interface IUserProfileRepository : IBaseCrudRepository<UserProfileModel>
    {
        Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId);
        Task<UserProfileModel?> FindByEmailAsync(string email);
        Task<UserProfileModel?> FindByFullNameAsync(string fullname);
        Task<UserProfileModel?> FindByUserLoginUsernameAsync(string username);
    }
}
