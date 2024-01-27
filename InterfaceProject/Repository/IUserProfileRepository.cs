using DataEntity.Model;

namespace InterfaceProject.Repository
{
    public interface IUserProfileRepository : IBaseCrudRepository<UserProfileModel>
    {
        public Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId);
        public Task<UserProfileModel?> FindByEmailAsync(string email);
        public Task<UserProfileModel?> FindByFullNameAsync(string fullname);
        public Task<UserProfileModel?> FindByUserLoginUsernameAsync(string username);
    }
}
