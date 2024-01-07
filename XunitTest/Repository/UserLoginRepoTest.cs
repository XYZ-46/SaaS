using DataEntity.User;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repository;

namespace XunitTest.Repository
{
    public class UserLoginRepoTest
    {
        [Fact]
        public void UserLogin_CRUD_Test()
        {
            // Arrange
            var testUserLoginModel = new UserLoginModel();

            var context = new Mock<DbContext>();
            var dbSetMock = new Mock<DbSet<UserLoginModel>>();

            context.Setup(x => x.Set<UserLoginModel>()).Returns(dbSetMock.Object);
            dbSetMock.Setup(x => x.Find(It.IsAny<int>())).Returns(testUserLoginModel);


            // Act
            //var repository = new UserLoginRepository(context.Object);
            //repository.Get(1);

        }
    }
}
