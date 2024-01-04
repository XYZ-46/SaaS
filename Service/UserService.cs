using DataEntity.User;
using InterfaceProject.Service;
using Repository.Database;
using System.Transactions;


namespace Service
{
    public class UserService(AzureDB azureDB) : IUserService
    {
        private readonly AzureDB _azureDB = azureDB;

        public async Task Register(UserRegisterRequest userRegisterParamReq)
        {
            var userLogin = UserMapper.MapToUserLogin(userRegisterParamReq);
            var userprofile = UserMapper.MapToUserProfile(userRegisterParamReq);

            using TransactionScope ts = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                await _azureDB.UserLoginModel.AddAsync(userLogin);
                await _azureDB.SaveChangesAsync();

                userprofile.UserLoginId = userLogin.Id;
                await _azureDB.UserProfileModel.AddAsync(userprofile);
                await _azureDB.SaveChangesAsync();

                ts.Complete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
