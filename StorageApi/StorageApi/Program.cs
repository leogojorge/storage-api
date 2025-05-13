using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using StorageApi.Filters;
using StorageApi.Infrastructure.Configuration;

namespace StorageApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "MyAuthCookie";
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/accessdenied";

                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;
            });
        builder.Services.AddAuthorization();

        builder.Services.AddMvc(
            config =>
            {
                config.Filters.Add(typeof(GenericExceptionFilter));
            });

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c => {
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.IgnoreObsoleteActions();
            c.IgnoreObsoleteProperties();
            c.CustomSchemaIds(type => type.FullName);
        });

        builder.ConfigureMongo();

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
            await context.Response.SendFileAsync("wwwroot/login.html");
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();

        app.UseDefaultFiles();

        app.UseStaticFiles();
    }
}
