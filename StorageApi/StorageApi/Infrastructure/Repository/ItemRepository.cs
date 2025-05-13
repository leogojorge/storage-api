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

        public async Task<PaginatedItemQueryResult> GetByFilters(GetItemByFilterRequest filters)
        {
            var filterBuilder = new FilterDefinitionBuilder<Item>();
            FilterDefinition<Item> filterDefinition = filterBuilder.Empty;

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

        public async Task<bool> Delete(string id)
        {
            var result = await this._collection.DeleteOneAsync(x => x.Id == id);
            return result.IsAcknowledged;
        }

        public async Task Update(Item item)
        {
            var filter = new FilterDefinitionBuilder<Item>().Eq(x => x.Id, item.Id);

            await this._collection.ReplaceOneAsync(filter, item);
        }
    }

    public interface IItemRepository
    {
        Task Save(Item item);
        
        Task Update(Item item);

        Task<Item> GetById(string id);

        Task<PaginatedItemQueryResult> GetByFilters(GetItemByFilterRequest filters);

        Task<List<Item>> GetAll();

        Task<bool> Delete(string id);
    }

    public record PaginatedItemQueryResult(List<Item> Items, int PageNumber, int PageSize, long ItemCount)
    {
    }
}
