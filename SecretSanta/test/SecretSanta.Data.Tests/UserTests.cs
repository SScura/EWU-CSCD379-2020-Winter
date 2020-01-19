using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        public async Task CreateUsers_VerifyInformationInDatabase_InfoConfirmed()
        {
            int userId1 = 1;
            int userId2 = 2;
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                
                User user1 = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya",
                    Id = userId1,
                    CreatedOn = DateTime.Now
                };
                using (var loggerFactory = new LoggerFactory())
                {
                    loggerFactory.CreateLogger("User Creation").LogInformation($"Creating user {user1}");
                }
                    applicationDbContext.User.Add(user1);

                User user2 = new User
                {
                    FirstName = "Sean",
                    LastName = "Scura",
                    Id = userId2,
                    CreatedOn = DateTime.Now
                };
                applicationDbContext.User.Add(user2);

                await applicationDbContext.SaveChangesAsync();
            }

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var user1 = await applicationDbContext.User.Where(a => a.Id == userId1).SingleOrDefaultAsync();
                var user2 = await applicationDbContext.User.Where(a => a.Id == userId2).SingleOrDefaultAsync();

                Assert.AreEqual(userId1, user1.Id);
                Assert.IsNotNull(user1.CreatedOn);
                Assert.AreEqual("Inigo", user1.FirstName);
                Assert.AreEqual("Montoya", user1.LastName);
                Assert.AreEqual(userId2, user2.Id);
                Assert.IsNotNull(user2.CreatedOn);
                Assert.AreEqual("Sean", user2.FirstName);
                Assert.AreEqual("Scura", user2.LastName);
            }
        }

        [TestMethod]
        public async Task CreateUser_FingerPrintDataSavedAsync_Confirmed()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            int userId1 = 1;
            int userId2 = 2;
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                User user1 = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya",
                    Id = userId1
                };
                applicationDbContext.User.Add(user1);

                User user2 = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya",
                    Id = userId2
                };
                applicationDbContext.User.Add(user2);

                await applicationDbContext.SaveChangesAsync();
            }

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user1 = await applicationDbContext.User.Where(a => a.Id == userId1).SingleOrDefaultAsync();
                var user2 = await applicationDbContext.User.Where(a => a.Id == userId2).SingleOrDefaultAsync();

                Assert.IsNotNull(userId1);
                Assert.AreEqual("imontoya", user1.CreatedBy);
                Assert.AreEqual("imontoya", user1.ModifiedBy);
                Assert.IsNotNull(userId2);
                Assert.AreEqual("imontoya", user2.CreatedBy);
                Assert.AreEqual("imontoya", user2.ModifiedBy);
            }
        }

        [TestMethod]
        public async Task CreateUserThenUpdate_FingerPrintDataUpdated_Confirmed()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            int userId1 = 1;
            int userId2 = 2;
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                User user1 = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya",
                    Id = userId1
                };
                applicationDbContext.User.Add(user1);

                User user2 = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya",
                    Id = userId2
                };
                applicationDbContext.User.Add(user2);

                await applicationDbContext.SaveChangesAsync();
            }

            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "sscura"));
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user1 = await applicationDbContext.User.Where(a => a.Id == userId1).SingleOrDefaultAsync();
                user1.FirstName = "Sean";
                user1.LastName = "Scura";

                await applicationDbContext.SaveChangesAsync();
            }

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var user1 = await applicationDbContext.User.Where(a => a.Id == userId1).SingleOrDefaultAsync();

                Assert.IsNotNull(user1);
                Assert.AreEqual("imontoya", user1.CreatedBy);
                Assert.AreEqual("sscura", user1.ModifiedBy);
            }
        }
    }
}