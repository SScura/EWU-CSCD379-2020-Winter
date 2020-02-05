using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogEngine.Api.Controllers
{
    //https://localhost/api/User
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService UserService { get; }

        public UserController(IUserService UserService)
        {
            UserService = UserService ?? throw new System.ArgumentNullException(nameof(UserService));
        }

        // GET: https://localhost/api/User
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            List<User> Users = await UserService.FetchAllAsync();
            return Users;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Get(int id)
        {
            if (await UserService.FetchByIdAsync(id) is User User)
            {
                return Ok(User);
            }
            return NotFound();
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] User value)
        {

        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}