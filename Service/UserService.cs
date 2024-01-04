using DataEntity.User;
using InterfaceProject.Service;
using Repository.Database;


namespace Service
{
    public class UserService(AzureDB azureDB) : IUserService
    {
        private readonly AzureDB _azureDB = azureDB;

        public void Register(UserRegisterRequest userRegisterParamReq)
        {
            var userLogin = UserRegisterRequest.MapToUserLoginModel();
            throw new NotImplementedException();
        }
    }
}
