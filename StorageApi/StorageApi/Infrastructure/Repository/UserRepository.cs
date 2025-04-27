using MongoDB.Driver;
using StorageApi.Domain;

namespace StorageApi.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private IMongoCollection<User> _collection;

        public UserRepository(IMongoDatabase database)
        {
            this._collection = database.GetCollection<User>(nameof(User));
        }

        public async Task<User> Create(User newUser)
        {
            await this._collection.InsertOneAsync(newUser);
            return newUser;
        }
        
        public async Task<User> Get(string user)
        {
            var result = await this._collection.FindAsync(x => x.Username == user);

            return result.FirstOrDefault();
        }
    }

    public interface IUserRepository
    {
        Task<User> Get(string user);

        Task<User> Create(User newUser);
    }
}
