using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
    public class UserController : Controller
    {
        public IHttpClientFactory ClientFactory { get; }

        public UserController(IHttpClientFactory clientFactory)
        {
            if (clientFactory is null)
                throw new System.ArgumentNullException(nameof(clientFactory));

            ClientFactory = clientFactory;
        }

        // GET: User
        public async Task<ActionResult> IndexAsync()
        {
            HttpClient httpClient = ClientFactory.CreateClient("SantaApi");
            var client = new UserClient(httpClient);

            ICollection<User> users = await client.GetAllAsync();

            return View(users);
        }
    }
}