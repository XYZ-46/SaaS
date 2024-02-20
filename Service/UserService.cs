using DataEntity.Mapper;
using DataEntity.Model;
using DataEntity.Pagination;
using DataEntity.Request;
using InterfaceProject.User;
using Microsoft.Extensions.Logging;
using System.Transactions;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Service
{
    public class UserService(ILogger<UserService> logger, IUserLoginRepository userLoginRepo, IUserProfileRepository userProfileRepo)
        : IUserService
    {
        private readonly IUserLoginRepository _userLoginRepo = userLoginRepo;

        private readonly IUserProfileRepository _userProfileRepo = userProfileRepo;
        private readonly ILogger<UserService> _logger = logger;

        public async Task Register(UserRegisterRequest userRegisterParam)
        {
            userRegisterParam.Password = BCryptNet.HashPassword(userRegisterParam.Password);

            var userLogin = userRegisterParam.MapToUserLogin();
            var userprofile = userRegisterParam.MapToUserProfile();

            using TransactionScope ts = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var userLoginSaved = await _userLoginRepo.InsertAsync(userLogin);

                userprofile.UserLoginId = userLoginSaved.Id;
                await _userProfileRepo.InsertAsync(userprofile);

                ts.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception={exceptionMessage}", ex.Message);
                throw;
            }
        }

        public PagingResponse<UserProfileModel> GetPagingData(PagingRequest pageRequest)
        {
            return PagingResponse<UserProfileModel>.ToPagedList(_userProfileRepo.BaseQuery(), pageRequest.PageIndex, pageRequest.PageSize);
        }


    }
}
