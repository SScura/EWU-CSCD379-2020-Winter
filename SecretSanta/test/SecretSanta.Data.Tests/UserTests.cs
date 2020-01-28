using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class UserTests : TestBase
    {
        [TestMethod]
        public async Task CreateSingleUser_ShouldSaveIntoDatabase()
        {
            int? userId1 = null;
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var user = UserSamples.CreateInigoMontoya();
                applicationDbContext.Users.Add(user);

                await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);

                userId1 = user.Id;
            }

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                User user1 =
                    await applicationDbContext.Users.Where(a => a.Id == userId1).SingleOrDefaultAsync();

                Assert.IsNotNull(user1);
                Assert.AreEqual(UserSamples.Inigo, user1.FirstName);
                Assert.AreEqual(UserSamples.Montoya, user1.LastName);
            }
        }
        [TestMethod]
        public async Task CreateMultipleUsers_ShouldSaveIntoDatabase()
        {
            int? userId1 = null;
            int? userId2 = null;
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var user = UserSamples.CreateInigoMontoya();
                applicationDbContext.Users.Add(user);

                var user2 = UserSamples.CreateLukeSkywalker();

                applicationDbContext.Users.Add(user2);

                await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);

                userId1 = user.Id;
                userId2 = user2.Id;
            }

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                User user1 =
                    await applicationDbContext.Users.Where(a => a.Id == userId1).SingleOrDefaultAsync();

                Assert.IsNotNull(user1);
                Assert.AreEqual(UserSamples.Inigo, user1.FirstName);
                Assert.AreEqual(UserSamples.Montoya, user1.LastName);

                User user2 =
                    await applicationDbContext.Users.Where(a => a.Id == userId2).SingleOrDefaultAsync();

                Assert.IsNotNull(user2);
                Assert.AreEqual(UserSamples.Luke, user2.FirstName);
                Assert.AreEqual(UserSamples.Skywalker, user2.LastName);
            }
        }

        [TestMethod]
        public async Task CreateSingleUser_ShouldSetFingerPrintDataOnInitialSave()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, UserSamples.MontoyaUserName));

            int? userId;
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = UserSamples.CreateInigoMontoya();
                applicationDbContext.Users.Add(user);

                await applicationDbContext.SaveChangesAsync();

                userId = user.Id;
            }
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = await applicationDbContext.Users.Where(a => a.Id == userId).SingleOrDefaultAsync();

                Assert.IsNotNull(user);
                Assert.AreEqual(UserSamples.MontoyaUserName, user.CreatedBy);
                Assert.AreEqual(UserSamples.MontoyaUserName, user.ModifiedBy);
            }
        }

        [TestMethod]
        public async Task CreateUser_UpdateUser_ShouldSetFingerPrintDataOnUpdate()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, UserSamples.MontoyaUserName));

            int? userId;

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = UserSamples.CreateInigoMontoya();

                applicationDbContext.Users.Add(user);

                var user2 = UserSamples.CreateLukeSkywalker();

                applicationDbContext.Users.Add(user2);

                await applicationDbContext.SaveChangesAsync();

                userId = user.Id;
            }

            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, UserSamples.SkywalkerUserName));
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = await applicationDbContext.Users.Where(a => a.Id == userId).SingleOrDefaultAsync();
                user.FirstName = "Luke";
                user.LastName = "Skywalker";

                await applicationDbContext.SaveChangesAsync();
            }

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = await applicationDbContext.Users.Where(a => a.Id == userId).SingleOrDefaultAsync();

                Assert.IsNotNull(user);
                Assert.AreEqual(UserSamples.MontoyaUserName, user.CreatedBy);
                Assert.AreEqual(UserSamples.SkywalkerUserName, user.ModifiedBy);
            }
        }
        [TestMethod]
        public async Task User_CanHaveMultipleGifts()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, UserSamples.MontoyaUserName));

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift1 = GiftSamples.CreateMotorcycle();
                var gift2 = GiftSamples.CreateCar();
                var user = UserSamples.CreateInigoMontoya();
                user.Gifts.Add(gift1);
                user.Gifts.Add(gift2);
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var users = await dbContext.Users.Include(u => u.Gifts).ToListAsync();

                Assert.AreEqual(1, users.Count);
                Assert.AreEqual(2, users[0].Gifts.Count);
            }
        }
        [TestMethod]
        public async Task User_CanBeJoinedToGroup()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
            hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, UserSamples.MontoyaUserName));

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var group = GroupSamples.Group1;
                var user = UserSamples.CreateInigoMontoya();
                user.UserGroups.Add(new UserGroup { User = user, Group = group });
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            using (var dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var users = await dbContext.Users.Include(u => u.UserGroups).ThenInclude(ug => ug.Group).ToListAsync();

                Assert.AreEqual(1, users.Count);
                Assert.AreEqual(1, users[0].UserGroups.Count);
                Assert.AreEqual(GroupSamples.Group1.Title, users[0].UserGroups[0].Group.Title);
            }
        }

        [TestMethod]
        public async Task CreateUser_DeleteUser_UserRemovedFromDb()
        {
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Users.Add(UserSamples.CreateInigoMontoya());
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var user = await dbContext.Users.SingleOrDefaultAsync();
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var users = await dbContext.Users.ToListAsync();
                Assert.AreEqual(0, users.Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void User_SetFirstNameToNull_ThrowsArgumentNullException()
        {
            _ = new User(null!, UserSamples.Montoya);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void User_SetLastNameToNull_ThrowsArgumentNullException()
        {
            _ = new User(UserSamples.Inigo, null!);
        }
    }
}