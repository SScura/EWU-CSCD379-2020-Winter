using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        [TestMethod]
        public async Task Gift_CanBeSavedToDatabase()
        {
            //Testing build
            // Arrange
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(GiftSamples.Motorcycle);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            // Act
            // Assert
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.ToListAsync();

                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual(GiftSamples.Motorcycle.Title, gifts[0].Title);
                Assert.AreEqual(GiftSamples.Motorcycle.Url, gifts[0].Url);
                Assert.AreEqual(GiftSamples.Motorcycle.Description, gifts[0].Description);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetTitleToNull_ThrowsArgumentNullException()
        {
            {
                _ = new Gift(null!, "", "", UserSamples.InigoMontoya);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetDescriptionToNull_ThrowsArgumentNullException()
        {

            _ = new Gift("", null!, "", UserSamples.InigoMontoya);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetUrlToNull_ThrowsArgumentNullException()
        {
            {
                _ = new Gift("", "", null!, UserSamples.InigoMontoya);
            }
        }
    }
}
