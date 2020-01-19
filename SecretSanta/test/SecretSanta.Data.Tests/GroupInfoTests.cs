using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class PostTests : TestBase
    {
        [TestMethod]
        public async Task AddGroupInfo_AddUser_AddGift_ForeignRelationshipConfirmed()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            var group1 = new Group
            {
                Name = "Group1",
                Id = 1,
                CreatedOn = DateTime.Now.Date,
            };
            var group2 = new Group
            {
                Name = "Group2",
                Id = 2,
                CreatedOn = DateTime.Now.Date,
            };
            User user = new User
            {
                Id = 1,
                FirstName = "Inigo",
                LastName = "Montoya",
                CreatedOn = DateTime.Now.Date
            };
            Gift gift = new Gift
            {
                Id = 1,
                Title = "Car",
                Url = "myurl.me",
                Description = "Red Ferrari",
                CreatedOn = DateTime.Now.Date
            };
            gift.User = user;
            user.GroupsInfo = new List<GroupInfo>();
            user.GroupsInfo.Add(new GroupInfo { User = user, Group = group1 });
            user.GroupsInfo.Add(new GroupInfo { User = user, Group = group2 });

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(gift);
                await dbContext.SaveChangesAsync();
            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var retrievedGift = await dbContext.Gifts.Where(g => g.Id == gift.Id)
                    .Include(u => u.User).ThenInclude(pt => pt.GroupsInfo)
                        .ThenInclude(at => at.Group).SingleOrDefaultAsync();

                Assert.IsNotNull(retrievedGift);
                Assert.AreEqual(2, retrievedGift.User.GroupsInfo.Count);
                Assert.IsNotNull(retrievedGift.User.GroupsInfo[0].Group);
                Assert.IsNotNull(retrievedGift.User.GroupsInfo[1].Group);
            }
        }
    }
}