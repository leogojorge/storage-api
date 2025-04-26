using MongoDB.Bson;
using MongoDB.Driver;
using StorageApi.Domain;

namespace StorageApi.Infrastructure.Repository
{
    public class ItemRepository : IItemRepository
    {
        private IMongoCollection<Item> _collection;

        public ItemRepository(IMongoDatabase database)
        {
            this._collection = database.GetCollection<Item>(nameof(Item));
        }

        public async Task<List<Item>> GetAll()
        {
            var result = await this._collection.FindAsync(_ => true);

            return result.ToList();
        }

        public async Task<Item> GetById(string id)
        {
            ObjectId objectId;
            try
            {
                objectId = new ObjectId(id);
            }
            catch (Exception)
            {
                return null;
            }

            var result = await this._collection.FindAsync(item => item.Id == objectId);
            return result.FirstOrDefault();
        }

        public async Task<List<Item>> GetByNameOrDescription(string value, int pageNumber = 1, int size = 10)
        {
            var filter = Builders<Item>.Filter.Text(value);

            var results = await this._collection.Find(filter)
                .Skip((pageNumber - 1) * size).Limit(size).ToListAsync();

            return results;
        }

        public async Task Save(Item item)
        {
            await this._collection.InsertOneAsync(item);
        }
    }

    public interface IItemRepository
    {
        Task Save(Item item);

        Task<Item> GetById(string id);

        Task<List<Item>> GetByNameOrDescription(string value, int pageNumber = 0, int size = 0);

        Task<List<Item>> GetAll();
    }
}
