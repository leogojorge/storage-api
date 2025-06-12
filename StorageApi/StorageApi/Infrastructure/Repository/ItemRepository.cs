using MongoDB.Driver;
using StorageApi.Controllers.Models.Request;
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

        public async Task<List<Item>> GetAll(string userId)
        {
            var result = await this._collection.FindAsync(item => item.UserId == userId);

            return result.ToList();
        }

        public async Task<Item> GetById(string id, string userId)
        {
            var result = await this._collection.FindAsync(item => item.Id == id && item.UserId == userId);
            return result.FirstOrDefault();
        }

        public async Task<PaginatedItemQueryResult> GetByFilters(GetItemByFilterRequest filters, string userId)
        {
            var filterBuilder = new FilterDefinitionBuilder<Item>();
            FilterDefinition<Item> filterDefinition = filterBuilder.Empty;
            
            filterDefinition = filterBuilder.Eq(x => x.UserId, userId);

            if (!string.IsNullOrWhiteSpace(filters.NameAndDescription))
                filterDefinition &= filterBuilder.Text(filters.NameAndDescription);

            if (!string.IsNullOrWhiteSpace(filters.PartNumber))
                filterDefinition &= filterBuilder.Eq(x => x.PartNumber, filters.PartNumber);

            if (!string.IsNullOrWhiteSpace(filters.Place))
                filterDefinition &= filterBuilder.Eq(x => x.Place, filters.Place);

            if (!string.IsNullOrWhiteSpace(filters.Supplier))
                filterDefinition &= filterBuilder.Eq(x => x.Supplier, filters.Supplier);
        

            long itemCount = this._collection.EstimatedDocumentCount();

            var items = await this._collection.Find(filterDefinition)
                .Skip((filters.PageNumber - 1) * filters.PageSize).Limit(filters.PageSize).ToListAsync();

            var paginatedResult = new PaginatedItemQueryResult(items, filters.PageNumber, filters.PageSize, itemCount);

            return paginatedResult;
        }

        public async Task Save(Item item)
        {
            await this._collection.InsertOneAsync(item);
        }

        public async Task<bool> Delete(string id, string userId)
        {
            var result = await this._collection.DeleteOneAsync(x => x.Id == id && x.UserId == userId);
            return result.IsAcknowledged;
        }

        public async Task Update(Item item, string userId)
        {
            var filterBuilder = new FilterDefinitionBuilder<Item>();
            FilterDefinition<Item> filterDefinition =
                filterBuilder.Eq(x => x.Id, item.Id) & 
                filterBuilder.Eq(x => x.UserId, userId);

            await this._collection.ReplaceOneAsync(filterDefinition, item);
        }
    }

    public interface IItemRepository
    {
        Task Save(Item item);
        
        Task Update(Item item, string userId);

        Task<Item> GetById(string id, string userId);

        Task<PaginatedItemQueryResult> GetByFilters(GetItemByFilterRequest filters, string userId);

        Task<List<Item>> GetAll(string userId);

        Task<bool> Delete(string id, string userId);
    }

    public record PaginatedItemQueryResult(List<Item> Items, int PageNumber, int PageSize, long ItemCount)
    {
    }
}
