using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogService.Abstractions;
using BlogService.Abstractions.Repositories;
using BlogService.DataModels;
using BlogService.Infastructure;
using BlogService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository  _commentRepository;

        public PostsController(IPostRepository postRepository,ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        // DELETE api/Posts/5 - deletes a specific Post
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _postRepository.RemovePost(id);
        }

        [NoCache]
        [HttpGet]
        public async Task<IEnumerable<Post>> Get()
        {
            return await _postRepository.GetAllPosts();
        }

        // GET api/Posts/5 - retrieves a specific Post using either Id or InternalId (BSonId)
        [HttpGet("{id}")]
        public async Task<Post> Get(string id)
        {
            return await _postRepository.GetPost(id) ?? new Post();
        }

        [HttpGet("{id}/comments")]
        public async Task<IEnumerable<Comment>> GetPostComments(string id)
        {
            return await _commentRepository.GetPostComments(id) ?? new List<Comment>();
        }



        // GET api/Posts/text/date/size
        // ex: /api/Posts/Test/2018-01-01/10000
        [NoCache]
        [HttpGet("{bodyText}/{updatedFrom}/{headerSizeLimit}")]
        public async Task<IEnumerable<Post>> Get(string bodyText,
            DateTime updatedFrom,
            long headerSizeLimit)
        {
            return await _postRepository.GetPost(bodyText, updatedFrom, headerSizeLimit)
                   ?? new List<Post>();
        }

        // POST api/Posts - creates a new Post
        [HttpPost]
        public void Post([FromBody] PostViewModel newPost)
        {
            _postRepository.AddPost(new Post
            {
                Id = newPost.Id,
                Body = newPost.Body,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                Tags = newPost.Tags,
                Header = newPost.Header
            });
        }

        // PUT api/Posts/5 - updates a specific Post
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
            _postRepository.UpdatePost(id, value);
        }
    }
}