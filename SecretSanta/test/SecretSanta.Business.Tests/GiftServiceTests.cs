using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System.Threading.Tasks;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftServiceTests : TestBase
    {

        [TestMethod]
        public async Task UpdateUser_ShouldSaveIntoDatabase()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(Options);

            IUserService service = new UserService(dbContext, Mapper);

            User user = UserSamples.CreateInigoMontoya();

            User user2 = UserSamples.CreateLukeSkywalker();

            await service.InsertAsync(user);
            await service.InsertAsync(user2);

            // Act
            using var dbContext2 = new ApplicationDbContext(Options);
            User fetchUser = await dbContext2.Users.SingleAsync(item => item.Id == user.Id);

            fetchUser.FirstName = "Princess";
            fetchUser.LastName = "Buttercup";

            using var dbContext3 = new ApplicationDbContext(Options);
            var service2 = new UserService(dbContext3, Mapper);
            await service2.UpdateAsync(2, fetchUser);

            // Assert
            using var dbContext4 = new ApplicationDbContext(Options);
            User savedUser = await dbContext4.Users.SingleAsync(item => item.Id == user.Id);
            var otherUser = await dbContext4.Users.SingleAsync(item => item.Id == 2);
            Assert.AreEqual(("Inigo", "Montoya"), (savedUser.FirstName, savedUser.LastName));
            Assert.AreNotEqual((savedUser.FirstName, savedUser.LastName), (otherUser.FirstName, otherUser.LastName));

        }
    }
}