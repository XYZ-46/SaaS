using DataEntity.User;

namespace InterfaceProject.Search
{
    public interface IUserProfileSearch
    {
        public Task<UserProfileModel?> FindByIDAsync(int id);
        public Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId);
        public Task<UserProfileModel?> FindByEmailAsync(string email);
        public Task<UserProfileModel?> FindByFullNameAsync(string fullname);

    }
}
