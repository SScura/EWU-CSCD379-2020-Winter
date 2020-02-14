using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SecretSanta.Data.SampleData;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : BaseControllerTests
    {
        [TestMethod]
        public async Task Get_ReturnsUsers()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            User user = UserSamples.CreateInigoMontoya();
            context.Users.Add(user);
            context.SaveChanges();

            HttpResponseMessage response = await Client.GetAsync("api/User");
            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            Business.Dto.User[] users = JsonSerializer.Deserialize<Business.Dto.User[]>(jsonData, options);

            Assert.AreEqual(user.Id, users[0].Id);
            Assert.AreEqual(user.FirstName, users[0].FirstName);
            Assert.AreEqual(user.LastName, users[0].LastName);
        }

        [TestMethod]
        public async Task Put_WithMissingId_NotFound()
        {
            Business.Dto.UserInput user = Mapper.Map<User, Business.Dto.UserInput>(UserSamples.CreateInigoMontoya());
            string jsonData = JsonSerializer.Serialize(user);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Client.PutAsync("api/User/42", stringContent);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WithValidId_Success()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            User userEntity1 = UserSamples.CreateInigoMontoya();
            User userEntity2 = UserSamples.CreateLukeSkywalker();

            context.Users.Add(userEntity1);
            context.Users.Add(userEntity2);

            context.SaveChanges();

            HttpResponseMessage response = await Client.DeleteAsync($"api/User/{userEntity1.Id}");
            response.EnsureSuccessStatusCode();

            using ApplicationDbContext contextAct = Factory.GetDbContext();

            List<User> usersAfter = await context.Users.ToListAsync();

            Assert.AreEqual(1, usersAfter.Count);
        }

        [TestMethod]
        public async Task Delete_WithInvalidId_NotFound()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            User userEntity = UserSamples.CreateInigoMontoya();

            context.Users.Add(userEntity);
            context.SaveChanges();

            HttpResponseMessage response = await Client.DeleteAsync($"api/User/{42}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}