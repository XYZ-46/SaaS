using DataEntity.User;

namespace InterfaceProject.Repository
{
    public interface IUserProfileRepository : IBaseCrudRepository<UserProfileModel>
    {
        public Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId);
        public Task<UserProfileModel?> FindByEmailAsync(string email);
        public Task<UserProfileModel?> FindByFullNameAsync(string fullname);
    }
}
