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
            var userInserted = await repo.InsertAsync(userLogin);
            var userfinded1 = await repo.FindByIdAsync(userInserted);
            var userfinded2 = await repo.FindByIdAsync(userInserted.Id);

            // Assert
            Assert.Single(_azureDB.UserLoginModel);
            Assert.False(userInserted.IsDelete);
            Assert.NotNull(userfinded1);
            Assert.NotNull(userfinded2);

            // ACT 2
            userInserted.Username = "ganti";
            var userUpdated = await repo.UpdateAsync(userInserted);

            // Assert 2
            Assert.Single(_azureDB.UserLoginModel);
            Assert.Equal(userUpdated.Username, userInserted.Username);
            Assert.False(userUpdated.IsDelete);

            // ACT 3
            var userDeleted = await repo.DeleteAsync(userLogin);

            // Assert 3
            Assert.Single(_azureDB.UserLoginModel);
            Assert.True(userDeleted);

        }
    }
}
