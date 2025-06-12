using MongoDB.Driver;
using StorageApi.Domain;
using StorageApi.Infrastructure.Repository;
using StorageApi.Infrastructure.Settings;

namespace StorageApi.Infrastructure.Configuration
{
    public static class MongoConfigurator
    {
        public static void ConfigureMongo(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();

                return client.GetDatabase("StorageApi");
            });

            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var mongoSettings = new MongoSettings();
                builder.Configuration.GetSection("MongoSettings").Bind(mongoSettings);

                return new MongoClient(mongoSettings.ConnectionString);
            });

            CreateIndexes(builder);

            builder.Services.AddScoped<IItemRepository, ItemRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }

        private static void CreateIndexes(WebApplicationBuilder builder)
        {
            var client = builder.Services.BuildServiceProvider().GetRequiredService<IMongoClient>();

            var database = client.GetDatabase("StorageApi");

            var collection = database.GetCollection<Item>(nameof(Item));

            var textIndexForNameAndDescription = new CreateIndexModel<Item>(Builders<Item>.IndexKeys
                .Text(m => m.Name)
                .Text(m => m.Description));

            var indexForItemIdAndUserId = new CreateIndexModel<Item>(Builders<Item>.IndexKeys
                .Ascending(m => m.Id)
                .Ascending(m => m.UserId));

            collection.Indexes.CreateOne(textIndexForNameAndDescription);
            collection.Indexes.CreateOne(indexForItemIdAndUserId);
        }
    }
}
