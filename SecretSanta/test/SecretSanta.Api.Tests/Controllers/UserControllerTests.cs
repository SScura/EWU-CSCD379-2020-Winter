using BlogEngine.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void Create_WithoutService_Success()
        {
            _ = new UserController(null!);
        }

        [TestMethod]
        public async Task GetById_WithExistingUser_Success()
        {
            var service = new UserService();
            var user = UserSamples.CreateInigoMontoya();

            await service.InsertAsync(user);

            var controller = new UserController(service);

            ActionResult<User> returnValue = await controller.Get(user.Id!.Value);

            Assert.IsTrue(returnValue.Result is OkObjectResult);
        }

        public class UserService : IUserService
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
                throw new NotImplementedException();
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
