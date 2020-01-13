using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void UserObject_CanPassValueToProperties_AcceptsValues()
        {
            int id = 7;
            string firstName = "Sean";
            string lastName = "Scura";
            var gifts = new List<Gift>();
            var user = new User(id, firstName, lastName, gifts);

            Assert.IsNotNull(user);
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(firstName, user.FirstName);
            Assert.AreEqual(lastName, user.LastName);
            Assert.IsNotNull(user.Gifts);
        }
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void UserObject_NullGifts_ThrowsException()
        {
            int id = 7;
            string firstName = "Sean";
            string lastName = "Scura";
            List<Gift> gifts = null!;
            var user = new User(id, firstName, lastName, gifts);
        }
    }
}