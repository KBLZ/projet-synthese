using EF_API.Services;
using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.Repositories;
using service = EF_API.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace EF_API
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- Configuration de la Base de Données ---
            var connectionString = builder.Configuration.GetConnectionString("OracleDb")
                                   ?? throw new InvalidOperationException("Connection string 'OracleDb' not found.");

            builder.Services.AddDbContext<DBContext>(options =>
                options.UseOracle(connectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            // --- Services de base ---
            builder.Services.AddControllers();

            // --- Documentation API (NSwag) ---
            // On n'utilise qu'une seule méthode pour éviter le conflit sur le nom de document 'v1'
            builder.Services.AddOpenApiDocument(settings =>
            {
                settings.Title = "EF API Documentation";
                settings.Version = "v1";
            });

            // --- Injection de Dépendances ---
            // Repositories
            builder.Services.AddScoped<ArrayRepository>();
            builder.Services.AddScoped<DescriptionRepository>();
            builder.Services.AddScoped<HistoricRepository>();
            builder.Services.AddScoped<NoteRepository>();

            // Services métiers
            builder.Services.AddScoped<service.Array>();
            builder.Services.AddScoped<service.Description>();
            builder.Services.AddScoped<service.Historic>();
            builder.Services.AddScoped<service.Note>();

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
           // {
                // 1. On définit où le JSON est généré
                app.UseOpenApi(options =>
                {
                    // On garde ce chemin si vous le souhaitez
                    options.Path = "/openapi/{documentName}.json";
                });

                /*// 2. On dit à Swagger UI où aller chercher ce JSON
                app.UseSwaggerUi(settings =>
                {
                    // On doit pointer vers le même chemin que UseOpenApi ci-dessus
                    // {documentName} sera remplacé par "v1" par défaut
                    settings.DocumentPath = "/openapi/{documentName}.json";
                    settings.Path = "/swagger"; // L'URL pour accéder à l'interface (ex: localhost:xxx/swagger)
                });*/

                // 3. Scalar détecte généralement automatiquement, mais restons cohérents
                app.MapScalarApiReference();
           // }
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}