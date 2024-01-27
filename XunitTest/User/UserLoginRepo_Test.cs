using DataEntity.Model;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Database;

namespace XunitTest.User
{
    public class UserLoginRepo_Test
    {
        private readonly AzureDB _azureDB = ContextDB_Generator.AzureGenerator();

        [Fact]
        public async Task UserLogin_CRUD_positiveTest()
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
            var userfinded3 = await repo.FindByUsernameAsync(userLogin.Username);

            // Assert
            Assert.Single(_azureDB.UserLoginModel);
            Assert.False(userInserted.IsDelete);
            Assert.NotNull(userfinded1);
            Assert.NotNull(userfinded2);
            Assert.NotNull(userfinded3);
            Assert.Equal(userInserted.Username, userInserted.Username);

            // ACT 2
            var OldUsername = userLogin.Username;
            var NewUsername = "ganti";
            userInserted.Username = NewUsername;
            var userUpdated = await repo.UpdateAsync(userInserted);
            var userfinded4 = await repo.FindByUsernameAsync(NewUsername);
            var userfinded41 = await repo.FindByUsernameAsync(OldUsername);

            // Assert 2
            Assert.Single(_azureDB.UserLoginModel);
            Assert.Equal(userUpdated.Username, userInserted.Username);
            Assert.NotEqual(userUpdated.Username, OldUsername);
            Assert.False(userUpdated.IsDelete);
            Assert.NotNull(userfinded4);
            Assert.Null(userfinded41);

            // ACT 3
            var userDeleted1 = await repo.DeleteAsync(userLogin);
            var userDeleted2 = await repo.DeleteAsync(userUpdated);
            var userfinded5 = await repo.FindByUsernameAsync(NewUsername);
            var userfinded6 = await repo.FindByUsernameAsync(userUpdated.Username);

            // Assert 3
            Assert.Single(_azureDB.UserLoginModel);
            Assert.True(userDeleted1);
            Assert.True(userDeleted2);
            Assert.Null(userfinded5);
            Assert.Null(userfinded6);

        }

        [Fact]
        public async Task UserLogin_CRUD_Negative_Test()
        {

            // Arrange
            var repo = new UserLoginRepository(_azureDB);
            var userLogin = new UserLoginModel();

            Task actInsert() => repo.InsertAsync(userLogin);
            await Assert.ThrowsAsync<DbUpdateException>(actInsert);
            Assert.False(_azureDB.UserLoginModel.Any());

            Task actUpdate() => repo.UpdateAsync(userLogin);
            await Assert.ThrowsAsync<DbUpdateException>(actUpdate);
            Assert.False(_azureDB.UserLoginModel.Any());

            var result = await repo.DeleteAsync(userLogin);
            Assert.False(_azureDB.UserLoginModel.Any());
            Assert.False(result);
        }
    }
}
