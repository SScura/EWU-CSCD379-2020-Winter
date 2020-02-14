using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.SampleData;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllerTests : BaseControllerTests
    {
        [TestMethod]
        public async Task Get_ReturnsGifts()
        {
            //Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Gift gift = GiftSamples.CreateMotorcycle();
            context.Gifts.Add(gift);
            context.SaveChanges();

            HttpResponseMessage response = await Client.GetAsync("api/Gift");
            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            Business.Dto.Gift[] gifts = JsonSerializer.Deserialize<Business.Dto.Gift[]>(jsonData, options);

            Assert.AreEqual(gift.Id, gifts[0].Id);
            Assert.AreEqual(gift.Title, gifts[0].Title);
            Assert.AreEqual(gift.Description, gifts[0].Description);
            Assert.AreEqual(gift.Url, gifts[0].Url);
            Assert.AreEqual(gift.UserId, gifts[0].UserId);
        }

        [TestMethod]
        public async Task Put_WithMissingId_NotFound()
        {
            Business.Dto.GiftInput gift = Mapper.Map<Gift, Business.Dto.GiftInput>(GiftSamples.CreateMotorcycle());
            string jsonData = JsonSerializer.Serialize(gift);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Client.PutAsync("api/Gift/42", stringContent);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_WithId_UpdatesGift()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Gift giftEntity = GiftSamples.CreateCar();

            context.Gifts.Add(giftEntity);
            context.SaveChanges();

            Business.Dto.GiftInput gift = Mapper.Map<Gift, Business.Dto.GiftInput>(giftEntity);
            gift.Title += "changed";
            gift.Description += "changed";
            gift.Url += "changed";

            string jsonData = JsonSerializer.Serialize(gift);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Client.PutAsync($"api/Gift/{giftEntity.Id}", stringContent);
            response.EnsureSuccessStatusCode();

            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift returnedGift = JsonSerializer.Deserialize<Business.Dto.Gift>(retunedJson, options);

            Assert.AreEqual(gift.Title, returnedGift.Title);
            Assert.AreEqual(gift.Description, returnedGift.Description);
            Assert.AreEqual(gift.Url, returnedGift.Url);
            Assert.AreEqual(gift.UserId, returnedGift.UserId);
        }

        [TestMethod]
        public async Task Delete_WithValidId_Success()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Gift giftEntity1 = GiftSamples.CreateMotorcycle();
            Gift giftEntity2 = GiftSamples.CreateMotorcycle();
            Gift giftEntity3 = GiftSamples.CreateCar();

            context.Gifts.Add(giftEntity1);
            context.Gifts.Add(giftEntity2);
            context.Gifts.Add(giftEntity3);

            context.SaveChanges();

            HttpResponseMessage response = await Client.DeleteAsync($"api/Gift/{giftEntity1.Id}");

            response.EnsureSuccessStatusCode();

            using ApplicationDbContext contextAct = Factory.GetDbContext();

            List<Gift> giftsAfter = await context.Gifts.ToListAsync();

            Assert.AreEqual(2, giftsAfter.Count);
        }

        [TestMethod]
        public async Task Delete_WithInvalidId_NotFound()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Gift giftEntity = GiftSamples.CreateMotorcycle();

            context.Gifts.Add(giftEntity);
            context.SaveChanges();

            HttpResponseMessage response = await Client.DeleteAsync($"api/Gift/{42}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        [DataRow(nameof(Business.Dto.GiftInput.Title))]
        [DataRow(nameof(Business.Dto.GiftInput.UserId))]
        public async Task Post_WithOutRequiredProperties_BadRequest(string propertyName)
        {
            Gift entity = GiftSamples.CreateCar();

            Business.Dto.GiftInput gift = Mapper.Map<Gift, Business.Dto.Gift>(entity);
            System.Type inputType = typeof(Business.Dto.GiftInput);
            System.Reflection.PropertyInfo? propInfo = inputType.GetProperty(propertyName);
            propInfo!.SetValue(gift, null);

            string jsonData = JsonSerializer.Serialize(gift);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Client.PostAsync($"api/Gift", stringContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}