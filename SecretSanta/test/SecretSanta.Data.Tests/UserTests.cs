using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class AuthorTests : TestBase
    {
        [TestMethod]
        public async Task CreateMultipleUsers_ShouldSaveIntoDatabase()
        {
            int? userId1 = null;
            int? userId2 = null;
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var user = UserSamples.InigoMontoya;
                applicationDbContext.Users.Add(user);

                var user2 = UserSamples.LukeSkywalker;

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
        public async Task CreateUser_ShouldSetFingerPrintDataOnInitialSave()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, UserSamples.MontoyaUserName));

            int? userId;
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = UserSamples.InigoMontoya;
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
        public async Task CreateUser_ShouldSetFingerPrintDataOnUpdate()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, UserSamples.MontoyaUserName));

            int? userId1 = null;
            int? userId2 = null;
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var user = UserSamples.InigoMontoya;
                applicationDbContext.Users.Add(user);

                var user2 = UserSamples.LukeSkywalker;

                applicationDbContext.Users.Add(user2);

                await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);

                userId1 = user.Id;
                userId2 = user2.Id;
            }

            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, UserSamples.SkywalkerUserName));
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user = await applicationDbContext.Users.Where(a => a.Id == userId1).SingleOrDefaultAsync();
                user = UserSamples.LukeSkywalker;

                await applicationDbContext.SaveChangesAsync();
            }

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user2 = await applicationDbContext.Users.Where(a => a.Id == userId2).SingleOrDefaultAsync();

                Assert.IsNotNull(user2);
                Assert.AreEqual(UserSamples.SkywalkerUserName, user2.CreatedBy);
                Assert.AreEqual(UserSamples.SkywalkerUserName, user2.ModifiedBy);
            }
        }
    }
}