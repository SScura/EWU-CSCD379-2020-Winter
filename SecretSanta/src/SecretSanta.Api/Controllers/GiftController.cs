using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Controllers
{
    //https://localhost/api/Gift
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : EntityController<Gift>
    {
        public GiftController(IGiftService entityService) : base(entityService)
        {
        }
    }
}