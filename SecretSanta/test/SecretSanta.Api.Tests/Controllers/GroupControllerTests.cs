using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
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
    public class GroupControllerTests : BaseControllerTests
    {
        [TestMethod]
        public async Task Get_ReturnsGroups()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Group group = GroupSamples.CreateEmployeeGroup();
            context.Groups.Add(group);
            context.SaveChanges();
            HttpResponseMessage response = await Client.GetAsync("api/Group");

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            Business.Dto.Group[] groups = JsonSerializer.Deserialize<Business.Dto.Group[]>(jsonData, options);

            Assert.AreEqual(group.Id, groups[0].Id);
            Assert.AreEqual(group.Title, groups[0].Title);
        }

        [TestMethod]
        public async Task Put_WithMissingId_NotFound()
        {
            Business.Dto.GroupInput group = Mapper.Map<Group, Business.Dto.GroupInput>(GroupSamples.CreateEmployeeGroup());
            string jsonData = JsonSerializer.Serialize(group);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Client.PutAsync("api/Group/42", stringContent);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WithValidId_Success()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Group groupEntity1 = GroupSamples.CreateEmployeeGroup();
            Group groupEntity2 = GroupSamples.CreateNonEmployeeGroup();

            context.Groups.Add(groupEntity1);
            context.Groups.Add(groupEntity2);

            context.SaveChanges();

            HttpResponseMessage response = await Client.DeleteAsync($"api/Group/{groupEntity1.Id}");

            response.EnsureSuccessStatusCode();

            using ApplicationDbContext contextAct = Factory.GetDbContext();

            List<Group> groupsAfter = await context.Groups.ToListAsync();

            Assert.AreEqual(1, groupsAfter.Count);
        }

        [TestMethod]
        public async Task Delete_WithInvalidId_NotFound()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Group groupEntity = GroupSamples.CreateEmployeeGroup();

            context.Groups.Add(groupEntity);
            context.SaveChanges();

            HttpResponseMessage response = await Client.DeleteAsync($"api/Group/{42}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}