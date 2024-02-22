using DataEntity.Model;
using DataEntity.Pagination;

namespace InterfaceProject.User
{
    public interface IUserProfileRepository : IBaseRepository<UserProfileModel>
    {
        Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId);
        Task<UserProfileModel?> FindByEmailAsync(string email);
        Task<UserProfileModel?> FindByFullNameAsync(string fullname);
        Task<UserProfileModel?> FindByUserLoginUsernameAsync(string username);
        IQueryable<UserProfileModel> PageQuery(PagingRequest pageRequest);
        PagingResponse<UserProfileModel> PageData(PagingRequest pageRequest);
    }
}
