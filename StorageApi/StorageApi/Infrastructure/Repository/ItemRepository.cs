using MongoDB.Driver;
using StorageApi.Domain;

namespace StorageApi.Infrastructure.Repository
{
    public class ItemRepository : IItemRepository
    {
        private IMongoCollection<Item> _collection;

        public ItemRepository(IMongoDatabase database)
        {
            this._collection = database.GetCollection<Item>("Item");
        }

        public async Task<Item> GetById(int id)
        {
            var result = await this._collection.FindAsync(item => item.Id == id);
            return result.FirstOrDefault();
        }

        public async Task Save(Item item)
        {
            await this._collection.InsertOneAsync(item);
        }
    }

    public interface IItemRepository
    {
        Task Save(Item item);

        Task<Item> GetById(int id);
    }

}
