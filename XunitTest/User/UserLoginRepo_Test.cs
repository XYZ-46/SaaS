using DataEntity.User;
using Repository.Database;
using Repository;

namespace XunitTest.User
{
    public class UserLoginRepo_Test
    {
        private readonly AzureDB _azureDB = ContextDB_Generator.AzureGenerator();

        [Fact]
        public async Task UserLogin_CRUD_Test()
        {
            // Arrange
            var repo = new UserLoginRepository(_azureDB);
            var userLogin = new UserLoginModel()
            {
                Username = Guid.NewGuid().ToString(),
                PasswordHash = Guid.NewGuid().ToString()
            };

            // ACT
            await repo.InsertAsync(userLogin);

            // Assert
            Assert.Single(_azureDB.UserLoginModel);

            // ACT 2
            userLogin.Username = "ganti";
            var userUpdated = await repo.UpdateAsync(userLogin);

            // Assert 2
            Assert.Single(_azureDB.UserLoginModel);
            Assert.Equal(userUpdated.Username,userLogin.Username);
            Assert.False(userUpdated.IsDelete);

            // ACT 3
            var userDeleted = await repo.DeleteAsync(userLogin);

            // Assert 3
            Assert.Single(_azureDB.UserLoginModel);
            Assert.True(userUpdated.IsDelete);

        }
    }
}
