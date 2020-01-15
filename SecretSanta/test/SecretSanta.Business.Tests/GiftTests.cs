using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftTests
    {
        const int Id = 7;
        const string FirstName = "Sean";
        const string LastName = "Scura";
        const string Title = "Title of Gift";
        const string Description = "A gift of some kind";
        const string Url = "www.myurl.com";

        [TestMethod]
        public void GiftObject_CanPassValueToProperties_AcceptsValues()
        {
            List<Gift> gifts = new List<Gift>();
            User user = new User(Id, FirstName, LastName, gifts);
            Gift gift = new Gift(Id, Title, Description, Url, user);

            Assert.IsNotNull(gift);
            Assert.AreEqual(Id, gift.Id);
            Assert.AreEqual<string>(Title, gift.Title);
            Assert.AreEqual<string>(Description, gift.Description);
            Assert.AreEqual<string>(Url, gift.Url);
            Assert.AreSame(user, gift.User);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(nameof(Gift.User), null)]
        [DataRow(nameof(Gift.Title), null)]
        [DataRow(nameof(Gift.Description), null)]
        [DataRow(nameof(Gift.Url), null)]
        public void AllGiftProperties_AssignNull_ThrowArgumentException(string propertyName, string? value)
        {
            SetPropertyOnGift(propertyName, value);
        }

        private static void SetPropertyOnGift(string propertyName, string? value)
        {
            List<Gift> gifts = new List<Gift>();
            User user = new User(Id, FirstName, LastName, gifts);
            Gift gift = new Gift(Id, Title, Description, Url, user);
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