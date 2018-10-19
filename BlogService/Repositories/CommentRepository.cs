using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogService.Abstractions;
using BlogService.Contexts;
using BlogService.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlogService.Repositories
{
    public class CommentRepository : BaseMongoRepository, ICommentRepository
    {
        private readonly MongoContext _context;
        private readonly ILogger<CommentRepository> _logger;


        public CommentRepository(IOptions<Settings> settings, ILogger<CommentRepository> logger)
        {
            _context = new MongoContext(settings);
            _logger = logger;
        }
        
        public async Task AddComment(Comment item)
        {
            try
            {
                await _context.Comments.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            try
            {
                return await _context.Comments
                    .Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<Comment> GetComment(string id)
        {
            try
            {
                var internalId = GetInternalId(id);
                return await _context.Comments
                    .Find(Comment => Comment.Id == id
                                  || Comment.InternalId == internalId)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<IEnumerable<Comment>> GetPostComments(string id)
        {
            try
            {
                return await _context.Comments
                    .Find(comment => comment.PostId == id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<bool> RemoveAllComments()
        {
            try
            {
                var actionResult
                    = await _context.Comments.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                       && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<bool> RemoveComment(string id)
        {
            try
            {
                var actionResult
                    = await _context.Comments.DeleteOneAsync(
                        Builders<Comment>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                       && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<bool> UpdateComment(string id, string body)
        {
            var filter = Builders<Comment>.Filter.Eq(s => s.Id, id);
            var update = Builders<Comment>.Update
                .Set(s => s.Body, body)
                .CurrentDate(s => s.UpdatedOn);

            try
            {
                var actionResult
                    = await _context.Comments.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                       && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<bool> UpdateComment(string id, Comment item)
        {
            try
            {
                var actionResult
                    = await _context.Comments
                        .ReplaceOneAsync(n => n.Id.Equals(id)
                            , item
                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                       && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
    }
}