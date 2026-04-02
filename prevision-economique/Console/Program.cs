
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PE_DAL.Oracle;
using PE_DAL.Oracle.Repositories;

var builder = Host.CreateApplicationBuilder(args);

// Configuration
builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

string connectionString = builder.Configuration.GetConnectionString("OracleDb") ?? "";

// Dependency Injection
builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseOracle(connectionString));

builder.Services.AddScoped<DTO_TableauRepository>();
builder.Services.AddScoped<DTO_DescriptionRepository>();
builder.Services.AddScoped<DTO_NoteRepository>();
builder.Services.AddScoped<DTO_HistoriqueRepository>();

using IHost host = builder.Build();

// Entry point logic
RunApp(host.Services);

static void RunApp(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var provider = scope.ServiceProvider;

    Console.WriteLine("Tentative de connexion à la base de données Oracle via EF Core...");

    try
    {
        var tableauRepo = provider.GetRequiredService<DTO_TableauRepository>();

        Console.WriteLine("Récupération des tableaux...");
        // Note: Cela échouera si la DB n'est pas accessible, mais le code est en place.
        var tableaux = tableauRepo.GetAll();

        Console.WriteLine($"Nombre de tableaux trouvés : {tableaux.Count}");
        foreach (var tab in tableaux)
        {
            Console.WriteLine($"- {tab.IdTableau}: {tab.TitreTableau}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la connexion ou de la récupération des données : {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Détails : {ex.InnerException.Message}");
        }
    }
}