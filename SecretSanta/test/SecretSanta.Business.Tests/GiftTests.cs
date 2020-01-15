using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftTests
    {
        const int id = 7;
        const string firstName = "Sean";
        const string lastName = "Scura";
        const string title = "Title of Gift";
        const string description = "A gift of some kind";
        const string url = "www.myurl.com";

        [TestMethod]
        public void GiftObject_CanPassValueToProperties_AcceptsValues()
        {
            List<Gift> gifts = new List<Gift>();
            User user = new User(id, firstName, lastName, gifts);
            Gift gift = new Gift(id, title, description, url, user);

            Assert.IsNotNull(gift);
            Assert.AreEqual(id, gift.Id);
            Assert.AreEqual(title, gift.Title!);
            Assert.AreEqual(description, gift.Description);
            Assert.AreEqual(url, gift.Url);
            Assert.IsNotNull(gift.User);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(nameof(Gift.User), null)]
        [DataRow(nameof(Gift.Title), null)]
        [DataRow(nameof(Gift.Description), null)]
        [DataRow(nameof(Gift.Url), null)]
        public void AllGiftProperties_AssignNull_ThrowArgumentException(string propertyName, string value)
        {
            SetPropertyOnGift(propertyName, value);
        }

        private static void SetPropertyOnGift(string propertyName, string value)
        {
            List<Gift> gifts = new List<Gift>();
            User user = new User(id, firstName, lastName, gifts);
            Gift gift = new Gift(id, title, description, url, user);
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