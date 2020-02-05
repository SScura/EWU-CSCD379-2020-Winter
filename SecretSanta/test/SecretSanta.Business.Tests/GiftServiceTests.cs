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
        public async Task CreateGift_ShouldSaveIntoDatabase()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(Options);

            IGiftService service = new GiftService(dbContext, Mapper);

            User user = UserSamples.CreateInigoMontoya();

            Gift gift = GiftSamples.CreateMotorcycle();

            await service.InsertAsync(gift);

            // Act

            // Assert
            Assert.IsNotNull(gift.Id);
            Assert.IsNotNull(user.Id);
            Assert.AreSame(gift.User, user);
            Assert.AreEqual(user.Id, gift.User.Id);
        }

        [TestMethod]
        public async Task FetchByIdGift_ShouldIncludeUser()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(Options);

            IGiftService service = new GiftService(dbContext, Mapper);

            User user = UserSamples.CreateInigoMontoya();

            Gift gift = GiftSamples.CreateMotorcycle();

            await service.InsertAsync(gift);

            // Act

            // Assert
            using var dbContext2 = new ApplicationDbContext(Options);
            service = new GiftService(dbContext, Mapper);
            gift = await service.FetchByIdAsync(gift.Id!.Value);

            Assert.IsNotNull(gift.User);
        }

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