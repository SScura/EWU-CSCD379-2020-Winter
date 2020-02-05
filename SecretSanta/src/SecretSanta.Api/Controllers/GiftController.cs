using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogEngine.Api.Controllers
{
    //https://localhost/api/Gift
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private IGiftService GiftService { get; }

        public GiftController(IGiftService GiftService)
        {
            GiftService = GiftService ?? throw new System.ArgumentNullException(nameof(GiftService));
        }

        // GET: https://localhost/api/Gift
        [HttpGet]
        public async Task<IEnumerable<Gift>> Get()
        {
            List<Gift> Gifts = await GiftService.FetchAllAsync();
            return Gifts;
        }

        // GET: api/Gift/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Gift>> Get(int id)
        {
            if (await GiftService.FetchByIdAsync(id) is Gift Gift)
            {
                return Ok(Gift);
            }
            return NotFound();
        }

        // POST: api/Gift
        [HttpPost]
        public void Post([FromBody] Gift value)
        {

        }

        // PUT: api/Gift/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Gift value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}