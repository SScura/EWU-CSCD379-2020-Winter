using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        [TestMethod]
        public async Task Gift_CanBeSavedToDatabase()
        {
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(GiftSamples.CreateMotorcycle());
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.ToListAsync();

                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual(GiftSamples.CreateMotorcycle().Title, gifts[0].Title);
                Assert.AreEqual(GiftSamples.CreateMotorcycle().Url, gifts[0].Url);
                Assert.AreEqual(GiftSamples.CreateMotorcycle().Description, gifts[0].Description);
            }
        }
        [TestMethod]
        public async Task CreateUser_AddGift_ShouldCreateForeignRelationship()
        {
            var user = UserSamples.CreateInigoMontoya();
            var gift = GiftSamples.CreateMotorcycle();
            gift.Url = ".hd.";
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                gift.User = user;

                dbContext.Gifts.Add(gift);

                await dbContext.SaveChangesAsync();
            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.Include(p => p.User).ToListAsync();
                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual(gift.Title, gifts[0].Title);
                Assert.IsNotNull(gifts[0].User);
                Assert.AreNotEqual(0, gifts[0].User.Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetTitleToNull_ThrowsArgumentNullException()
        {
            {
                _ = new Gift(null!, "", "", UserSamples.CreateInigoMontoya());
            }
        }

        [TestMethod]
        public async Task CreateGift_UpdateTitle_TitleIsUpdatedInDb()
        {
            string updatedTitle = "New Title";
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(GiftSamples.CreateMotorcycle());
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gift = await dbContext.Gifts.Include(g => g.User).SingleOrDefaultAsync();
                Assert.AreEqual(GiftSamples.CreateMotorcycle().Title, gift.Title);
                Assert.AreEqual(GiftSamples.CreateMotorcycle().Description, gift.Description);
                Assert.AreEqual(GiftSamples.CreateMotorcycle().Url, gift.Url);
                gift.Title = updatedTitle;
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gift = await dbContext.Gifts.Include(g => g.User).SingleOrDefaultAsync();
                Assert.AreEqual(updatedTitle, gift.Title);
            }
        }
        [TestMethod]
        public async Task CreateGift_UpdateDescription_TitleIsUpdatedInDb()
        {
                            string updatedDescription = "New Description";
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(GiftSamples.CreateMotorcycle());
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gift = await dbContext.Gifts.Include(g => g.User).SingleOrDefaultAsync();
                Assert.AreEqual(GiftSamples.CreateMotorcycle().Title, gift.Title);
                Assert.AreEqual(GiftSamples.CreateMotorcycle().Description, gift.Description);
                Assert.AreEqual(GiftSamples.CreateMotorcycle().Url, gift.Url);
                gift.Description = updatedDescription;
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gift = await dbContext.Gifts.Include(g => g.User).SingleOrDefaultAsync();
                Assert.AreEqual(updatedDescription, gift.Description);
            }
        }

        [TestMethod]
        public async Task CreateGift_DeleteGift_GiftRemovedFromDb()
        {
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(GiftSamples.CreateCar());
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gift = await dbContext.Gifts.SingleOrDefaultAsync();
                dbContext.Gifts.Remove(gift);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.ToListAsync();
                Assert.AreEqual(0, gifts.Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetDescriptionToNull_ThrowsArgumentNullException()
        {

            _ = new Gift("", null!, "", UserSamples.CreateInigoMontoya());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetUrlToNull_ThrowsArgumentNullException()
        {
            {
                _ = new Gift("", "", null!, UserSamples.CreateInigoMontoya());
            }
        }

    }

}