using BlogService.DataModels;
using BlogService.Domain.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BlogService.Contexts
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("comments");

        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("posts");
    }
} 