using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserTests
    {
        const int Id = 7;
        const string FirstName = "Sean";
        const string LastName = "Scura";

        [TestMethod]
        [DataRow(nameof(User.FirstName), FirstName)]
        [DataRow(nameof(User.LastName), LastName)]
        [DataRow(nameof(User.Id), Id)]
        public void UserObject_CanPassValueToProperties_AcceptsValues(string propertyName, object value)
        {
            List<Gift> gifts = new List<Gift>();
            User user = new User(Id, FirstName, LastName, gifts);

            Assert.IsNotNull(user);
            Assert.AreEqual(value, GetProperty(propertyName));
            Assert.IsNotNull(user.Gifts);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserObject_NullGifts_ThrowsException()
        {
            List<Gift> gifts = null!;
            User user = new User(Id, FirstName, LastName, gifts);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(nameof(User.FirstName), null)]
        [DataRow(nameof(User.LastName), null)]
        public void AllUserGetSetProperties_AssignNull_ThrowArgumentException(string propertyName, string value)
        {
            SetPropertyOnUser(propertyName, value);
        }

        private static void SetPropertyOnUser(string propertyName, string value)
        {
            List<Gift> gifts = new List<Gift>();
            User user = new User(Id, FirstName, LastName, gifts);
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
        private static object? GetProperty(string propertyName)
        {
            List<Gift> gifts = new List<Gift>();
            User user = new User(Id, FirstName, LastName, gifts);
            Type type = user.GetType();
            object? returnValue = null;

            var propertyInfo = type.GetProperty(propertyName)!;
            try
            {
                returnValue =  propertyInfo.GetValue(user);
            }
            catch (TargetInvocationException exception)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException!).Throw();
            }
            return returnValue;
        }
    }
}