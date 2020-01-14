using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserTests
    {
        const int id = 7;
        const string firstName = "Sean";
        const string lastName = "Scura";

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserObject_NullGifts_ThrowsException()
        {
            int id = 7;
            string firstName = "Sean";
            string lastName = "Scura";
            List<Gift> gifts = null!;
            var user = new User(id, firstName, lastName, gifts);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(nameof(User.FirstName), null)]
        [DataRow(nameof(User.LastName), null)]
        public void AssignNull_AllUserProperties_ThrowArgumentException(string propertyName, string value)
        {
            SetPropertyOnGift(propertyName, value);
        }

        private static void SetPropertyOnGift(string propertyName, string value)
        {
            List<Gift> gifts = new List<Gift>();
            User user = new User(id, firstName, lastName, gifts);
            Type type = user.GetType();

            var propertyInfo = type.GetProperty(propertyName)!;
            try
            {
                propertyInfo.SetValue(user, value);
            }
            catch (TargetInvocationException exception)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException!).Throw();
            }

        }
    }
}