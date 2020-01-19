using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        [TestMethod]
        public async Task CreateGiftWithUser_ForeignRelationshipExists_Confirmed()
        {
            Gift gift = new Gift
            {
                Title = "Car",
                Description = "Ferrari",
                Url = "test.me",
                ModifiedBy = "Inigo",
                CreatedBy = "Montoya"
            };
            User user = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                gift.User = user;
                dbContext.Gifts.Add(gift);
                await dbContext.SaveChangesAsync();
            }

            using (ApplicationDbContext applicationDbContext = new ApplicationDbContext(Options))
            {
                var gifts = await applicationDbContext.Gifts.Include(g => g.User).ToListAsync();
                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual(gift.Title, gifts[0].Title);
                Assert.AreNotEqual(0, gifts[0].Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(nameof(Gift.Title), null)]
        public void Gift_SetTitleToNull_ThrowsArgumentNullException(string propertyName, string? value)
        {
            {
                SetPropertyOnGift(propertyName, value);
            }
            static void SetPropertyOnGift(string propertyName, string? value)
            {
                Gift gift = new Gift
                {
                    Id = 1,
                    Description = "Red Ferrari",
                };
                Type type = gift.GetType();

                var propertyInfo = type.GetProperty(propertyName)!;
                try
                {
                    propertyInfo.SetValue(gift, value);
                }
                catch (TargetInvocationException exception)
                {
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException!).Throw();
                }
            }         
        }
    }
}