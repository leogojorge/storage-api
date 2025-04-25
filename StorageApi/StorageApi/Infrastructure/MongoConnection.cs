using MongoDB.Driver;
using StorageApi.Domain;

namespace StorageApi.Infrastructure
{
    public class MongoConnection
    {
        public MongoConnection()
        {
        }

        public void CreateMongoConnection()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("MyAppDb");
            var collection = database.GetCollection<Item>("Items");
        }
        
    }
}
