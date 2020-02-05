using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlogEngine.Api.Controllers
{
    //https://localhost/api/Group
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService GroupService { get; }

        public GroupController(IGroupService GroupService)
        {
            GroupService = GroupService ?? throw new System.ArgumentNullException(nameof(GroupService));
        }

        //// GET: https://localhost/api/Group
        //[HttpGet]
        //public async Task<IEnumerable<Group>> Get()
        //{
        //    List<Group> Groups = await GroupService.FetchAllAsync();
        //    return Groups;
        //}

        // GET: api/Group/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> Get(int id)
        {
            if (await GroupService.FetchByIdAsync(id) is { } Group)
            {
                return Ok(Group);
            }
            return NotFound();
        }

        // POST: api/Group
        [HttpPost]
        public void Post([FromBody] Group value)
        {

        }

        // PUT: api/Group/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Group value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}