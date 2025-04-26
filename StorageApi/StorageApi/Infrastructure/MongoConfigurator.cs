using MongoDB.Driver;
using StorageApi.Domain;
using StorageApi.Infrastructure.Repository;
using StorageApi.Infrastructure.Settings;

namespace StorageApi.Infrastructure
{
    public static class MongoConfigurator
    {
        public static void ConfigureMongo(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();

                var database = client.GetDatabase("StorageApi");

                var collection = database.GetCollection<Item>(nameof(Item));

                var indexModel = new CreateIndexModel<Item>(Builders<Item>.IndexKeys
                    .Text(m => m.Name)
                    .Text(m => m.Description));

                collection.Indexes.CreateOne(indexModel);

                return database;
            });

            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var mongoSettings = new MongoSettings();
                builder.Configuration.GetSection("MongoSettings").Bind(mongoSettings);

                return new MongoClient(mongoSettings.ConnectionString);
            });

            builder.Services.AddScoped<IItemRepository, ItemRepository>();

            
        }
        
    }
}
