using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogService.Abstractions;
using BlogService.Infastructure;
using BlogService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // DELETE api/Comments/5 - deletes a specific Comment
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _commentRepository.RemoveComment(id);
        }

        [NoCache]
        [HttpGet]
        public async Task<IEnumerable<Comment>> Get()
        {
            return await _commentRepository.GetAllComments();
        }

        // GET api/Comments/5 - retrieves a specific Comment using either Id or InternalId (BSonId)
        [HttpGet("{id}")]
        public async Task<Comment> Get(string id)
        {
            return await _commentRepository.GetComment(id) ?? new Comment();
        }

        
        // Comment api/Comments - creates a new Comment
        [HttpPost]
        public void Comment([FromBody] CommentParam newComment)
        {
            _commentRepository.AddComment(new Comment
            {
                Id = newComment.Id,
                Body = newComment.Body,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                Email = newComment.Email,
                FirstName = newComment.FirstName,
                LastName = newComment.LastName,
                Website = newComment.Website,
                PostId =  newComment.PostId

            });
        }

        // PUT api/Comments/5 - updates a specific Comment
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
            _commentRepository.UpdateComment(id, value);
        }
    }
}