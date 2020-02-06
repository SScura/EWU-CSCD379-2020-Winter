using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void CreateService_WithUserController_Success()
        {
            var service = new UserService();
            _ = new UserController(service);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Success()
        {
            _ = new UserController(null!);
        }

        private class UserService : IUserService
        {
            private Dictionary<int, User> Items { get; } = new Dictionary<int, User>();
            public Task<bool> DeleteAsync(int id)
            {
                throw new NotImplementedException();
            }

            public Task<List<User>> FetchAllAsync()
            {
                throw new NotImplementedException();
            }

            public Task<User> FetchByIdAsync(int id)
            {
                if (Items.TryGetValue(id, out User? user))
                {
                    Task<User> t1 = Task.FromResult(user);
                    return t1;
                }
                Task<User> t2 = Task.FromResult<User>(null!);
                return t2;
            }

            public Task<User> InsertAsync(User entity)
            {
                int id = Items.Count + 1;
                Items[id] = new TestUser(entity, id);
                return Task.FromResult(Items[id]);
            }

            public Task<User?> UpdateAsync(int id, User entity)
            {
                throw new NotImplementedException();
            }
            public class TestUser : User
            {
                public TestUser(User user, int id)
                    : base(user.FirstName, user.LastName)
                {
                    Id = id;
                }
            }
        }
    }
}
