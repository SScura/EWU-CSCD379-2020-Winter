using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using System;
using SecretSanta.Data.Tests;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : BaseControllerTests
    {
        [TestMethod]
        public async Task Get_ReturnsUsers()
        {
            //Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            User user = UserSamples;
            context.Users.Add(user);
            context.SaveChanges();
    }
    //Act
    //Justification: URL is type string, not type URI in this project
#pragma warning disable CA2234 // Pass system uri objects instead of strings
    HttpResponseMessage response = await Client.GetAsync("api/User");
#pragma warning restore CA2234 // Pass system uri objects instead of strings

    public class UserInMemoryService : InMemoryEntityService<User>, IUserService
    {
        //Assert
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
        //Arrange
        Business.Dto.UserInput user = Mapper.Map<User, Business.Dto.UserInput>(SampleData.CreateJonDoe());
        string jsonData = JsonSerializer.Serialize(user);

        using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        //Act
        //Justification: URL is type string, not type URI in this project
#pragma warning disable CA2234 // Pass system uri objects instead of strings
        HttpResponseMessage response = await Client.PutAsync("api/User/42", stringContent);
#pragma warning restore CA2234 // Pass system uri objects instead of strings

        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task Delete_WithValidId_Success()
    {
        // Arrange
        using ApplicationDbContext context = Factory.GetDbContext();
        User userEntity1 = SampleData.CreateJonDoe();
        User userEntity2 = SampleData.CreateBrandonFields();
        User userEntity3 = SampleData.CreateRiverWillis();

        context.Users.Add(userEntity1);
        context.Users.Add(userEntity2);
        context.Users.Add(userEntity3);

        context.SaveChanges();

        //Act
        //Justification: URL is type string, not type URI in this project
#pragma warning disable CA2234 // Pass system uri objects instead of strings
        HttpResponseMessage response = await Client.DeleteAsync($"api/User/{userEntity1.Id}");
#pragma warning restore CA2234 // Pass system uri objects instead of strings

        //Assert
        response.EnsureSuccessStatusCode();

        using ApplicationDbContext contextAct = Factory.GetDbContext();

        List<Data.User> usersAfter = await context.Users.ToListAsync();

        Assert.AreEqual(2, usersAfter.Count);
    }

    [TestMethod]
    public async Task Delete_WithInvalidId_NotFound()
    {
        // Arrange
        using ApplicationDbContext context = Factory.GetDbContext();
        User userEntity = SampleData.CreateJonDoe();

        context.Users.Add(userEntity);
        context.SaveChanges();

        //Act
        //Justification: URL is type string, not type URI in this project
#pragma warning disable CA2234 // Pass system uri objects instead of strings
        HttpResponseMessage response = await Client.DeleteAsync($"api/User/{42}");
#pragma warning restore CA2234 // Pass system uri objects instead of strings

        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
}