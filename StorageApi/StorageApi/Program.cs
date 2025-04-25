using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using StorageApi.Infrastructure.Repository;

namespace StorageApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        RegisterMongo(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();

        app.MapGet("/", async context => //map the first page to be opened on index.html 
        {
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync("wwwroot/index.html");
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void RegisterMongo(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMongoClient>(sp =>
        {
            var connectionString = builder.Configuration.GetSection("MongoSettings").GetValue<string>("ConnectionString");
            
            return new MongoClient(connectionString);
        });

        builder.Services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase("StorageApi");
        });

        // Register the repository
        builder.Services.AddScoped<IItemRepository, ItemRepository>();
    }

}
