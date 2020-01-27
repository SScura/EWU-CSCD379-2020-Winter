using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GroupTests : TestBase
    {
        [TestMethod]
        public async Task Group_CanBeSavedToDatabase()
        {
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Groups.Add(GroupSamples.Group1);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                var groups = await dbContext.Groups.ToListAsync();

                Assert.AreEqual(1, groups.Count);
                Assert.AreEqual(GroupSamples.Group1.Title, groups[0].Title);
            }
        }
    }
}