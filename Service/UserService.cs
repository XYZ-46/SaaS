using DataEntity.User;
using InterfaceProject.Repository;
using InterfaceProject.Service;
using Microsoft.Extensions.Logging;
using Repository;
using Repository.Database;
using System.Transactions;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Service
{
    public class UserService(ILogger<UserService> logger, IUserLoginRepo userLoginRepo, IUserProfileRepo userProfileRepo) : IUserService
    {
        private readonly IUserLoginRepo _userLoginRepo = userLoginRepo;
        private readonly IUserProfileRepo _userProfileRepo = userProfileRepo;
        private readonly ILogger<UserService> _logger = logger;

        public async Task Register(UserRegisterRequest userRegisterParamReq)
        {
            userRegisterParamReq.Password = BCryptNet.HashPassword(userRegisterParamReq.Password);

            var userLogin = UserMapper.MapToUserLogin(userRegisterParamReq);
            var userprofile = UserMapper.MapToUserProfile(userRegisterParamReq);

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
    }
}
