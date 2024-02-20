using DataEntity.Model;

namespace InterfaceProject.User
{
    public interface IUserProfileRepository : IBaseRepository<UserProfileModel>
    {
        Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId);
        Task<UserProfileModel?> FindByEmailAsync(string email);
        Task<UserProfileModel?> FindByFullNameAsync(string fullname);
        Task<UserProfileModel?> FindByUserLoginUsernameAsync(string username);
    }
}
