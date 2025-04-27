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
            var result = await this._collection.FindAsync(item => item.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<PaginatedItemQueryResult> GetByNameOrDescription(string value, int pageNumber = 1, int pageSize = 10)
        {
            var filter = Builders<Item>.Filter.Text(value);

            long itemCount = this._collection.CountDocuments(filter);

            var items = await this._collection.Find(filter)
                .Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

            var paginatedResult = new PaginatedItemQueryResult(items, pageNumber, pageSize, itemCount);

            return paginatedResult;            
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

        Task<PaginatedItemQueryResult> GetByNameOrDescription(string value, int pageNumber = 0, int size = 0);

        Task<List<Item>> GetAll();
    }

    public record PaginatedItemQueryResult(List<Item> Items, int pageNumber, int PageSize, long ItemCount)
    {
    }
}
