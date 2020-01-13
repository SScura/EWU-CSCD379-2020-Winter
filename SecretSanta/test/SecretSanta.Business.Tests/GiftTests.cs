using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftTests
    {
        [TestMethod]
        public void GiftObject_CanPassValueToProperties_AcceptsValues()
        {
            int id = 7;
            string firstName = "Sean";
            string lastName = "Scura";
            string title = "Title";
            string description = "Scura";
            string url = "www.myurl.com";
            var gifts = new List<Gift>();
            var user = new User(id, firstName, lastName, gifts);
            var gift = new Gift(id, title, description, url, user);

            Assert.IsNotNull(gift);
            Assert.AreEqual(id, gift.Id);
            Assert.AreEqual(title, gift.Title);
            Assert.AreEqual(description, gift.Description);
            Assert.AreEqual(url, gift.Url);
            Assert.IsNotNull(gift.User);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiftObject_NullUser_ThrowsException()
        {
            int id = 7;
            string title = "Title";
            string description = "Scura";
            string url = "www.myurl.com";
            User user = null!;
            var gift = new Gift(id, title, description, url, user);
        }
    }
}