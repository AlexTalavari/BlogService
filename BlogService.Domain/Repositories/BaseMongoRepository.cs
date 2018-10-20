using MongoDB.Bson;

namespace BlogService.Domain.Repositories
{
    public class BaseMongoRepository
    {
        protected ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out var internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
    }
}