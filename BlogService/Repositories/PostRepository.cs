using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Abstractions;
using BlogService.Contexts;
using BlogService.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BlogService.Repositories
{
    public class PostRepository : BaseMongoRepository, IPostRepository
    {
        private readonly MongoContext _context;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(IOptions<Settings> settings, ILogger<PostRepository> logger)
        {
            _context = new MongoContext(settings);
            _logger = logger;
        }

        public async Task AddPost(Post item)
        {
            try
            {
                await _context.Posts.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            try
            {
                return await _context.Posts
                    .Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after Id or InternalId (BSonId value)
        //
        public async Task<Post> GetPost(string id)
        {
            try
            {
                var internalId = GetInternalId(id);
                return await _context.Posts
                    .Find(Post => Post.Id == id
                                  || Post.InternalId == internalId)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        // query after body text, updated time, and header image size
        //
        public async Task<IEnumerable<Post>> GetPost(string bodyText, DateTime updatedFrom, long headerSizeLimit)
        {
            try
            {
                var query = _context.Posts.Find(Post => Post.Body.Contains(bodyText) &&
                                                        Post.UpdatedOn >= updatedFrom &&
                                                        Post.Header.ImageSize <= headerSizeLimit);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
        

        public async Task<bool> RemoveAllPosts()
        {
            try
            {
                var actionResult
                    = await _context.Posts.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                       && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<bool> RemovePost(string id)
        {
            try
            {
                var actionResult
                    = await _context.Posts.DeleteOneAsync(
                        Builders<Post>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                       && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<bool> UpdatePost(string id, string body)
        {
            var filter = Builders<Post>.Filter.Eq(s => s.Id, id);
            var update = Builders<Post>.Update
                .Set(s => s.Body, body)
                .CurrentDate(s => s.UpdatedOn);

            try
            {
                var actionResult
                    = await _context.Posts.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                       && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public async Task<bool> UpdatePost(string id, Post item)
        {
            try
            {
                var actionResult
                    = await _context.Posts
                        .ReplaceOneAsync(n => n.Id.Equals(id)
                            , item
                            , new UpdateOptions {IsUpsert = true});
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