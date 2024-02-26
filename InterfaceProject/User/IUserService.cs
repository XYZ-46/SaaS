using DataEntity.Model;
using DataEntity.Pagination;
using DataEntity.Request;

namespace InterfaceProject.User
{
    public interface IUserService
    {
        Task<UserProfileModel?> FindUserByID(int id);
        Task Register(UserRegisterRequest userRegisterParam);
        PagingResponse<UserProfileModel> GetPagingData(PagingRequest pageRequest);
    }
}
