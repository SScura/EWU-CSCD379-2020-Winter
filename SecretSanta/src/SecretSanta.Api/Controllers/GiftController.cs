using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Business;
using BlogEngine.Business.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private IPostService PostService { get; }

        public PostController(IPostService postService)
        {
            PostService = postService ?? throw new System.ArgumentNullException(nameof(postService));
        }

        // GET: https://localhost/api/Post
        [HttpGet]
        public async Task<IEnumerable<Post>> Get()
        {
            List<Post> posts = await PostService.FetchAllAsync();
            return posts;
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Post>> Get(int id)
        {
            if (await PostService.FetchByIdAsync(id) is Post post)
            {
                return Ok(post);
            }
            return NotFound();
        }

        // POST: api/Post
        [HttpPost]
        public async Task<Post> Post(PostInput value)
        {
            return await PostService.InsertAsync(value);
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Post>> Put(int id, PostInput value)
        {
            if (await PostService.UpdateAsync(id, value) is Post post)
            {
                return post;
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            if (await PostService.DeleteAsync(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}