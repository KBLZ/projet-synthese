<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
=======
﻿
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
>>>>>>> ec2d882f7b2ce5fec3d5b24ddcb42ddbf52a6740
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PE_DAL.Oracle;
<<<<<<< HEAD
using PE_DAL.Oracle.Context;
=======
>>>>>>> ec2d882f7b2ce5fec3d5b24ddcb42ddbf52a6740
using PE_DAL.Oracle.Repositories;

var builder = Host.CreateApplicationBuilder(args);

<<<<<<< HEAD
class Program
{

    public const string STRING_ALIGNMENT = "{0,-10} | {1,-10} | {2,-90} | {3,-25}|";
    public const string STRING_ALIGNMENT1 = "{0,-10} | {1,-100} | {2,-40} | {3,-10}|";

    static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("OracleDb")
            ?? throw new InvalidOperationException("Connection string 'OracleDb' not found.");

        builder.Services.AddDbContext<PE_DBContext>(options =>
            options.UseOracle(connectionString)
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );

        builder.Services.AddScoped<DTO_DescriptionRepository>();
        builder.Services.AddScoped<DTO_HistoriqueRepository>();
        builder.Services.AddScoped<DTO_NoteRepository>();
        builder.Services.AddScoped<DTO_TableauRepository>();

        using var host = builder.Build();
        using var scope = host.Services.CreateScope();

        var repo1 = scope.ServiceProvider.GetRequiredService<DTO_DescriptionRepository>();
        var allDescriptions = repo1.LireDonnees<DTO_Description>("TAB21_17");
        var repo2 = scope.ServiceProvider.GetRequiredService<DTO_HistoriqueRepository>();
        var allHisto = repo2.LireDonnees<DTO_Historique>();
        var repo3 = scope.ServiceProvider.GetRequiredService<DTO_NoteRepository>();
        var allNotes = repo3.LireDonnees<DTO_Note>();
        var repo4 = scope.ServiceProvider.GetRequiredService<DTO_TableauRepository>();
        var allTab = repo4.LireDonnees<DTO_Tableau>();

        Console.ForegroundColor = ConsoleColor.DarkYellow;

        Console.WriteLine(String.Format(STRING_ALIGNMENT, "IDTABLEAU", "POSITION", "DESCRIPTION", "LIGNE1TAB"));
        Console.WriteLine(new string('-', 145));

        Console.ForegroundColor = ConsoleColor.Gray;

        foreach (DTO_Description desc in allDescriptions)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(String.Format(
                STRING_ALIGNMENT,
                desc.IdTableau?.ToString() ?? "",
                desc.Position?.ToString() ?? "",
                desc.DescriptionTexte ?? string.Empty,
                desc.Ligne1Tableau ?? string.Empty
            ));

            Console.WriteLine(new string('-', 145));
            Console.ForegroundColor = ConsoleColor.Gray;

        }


        Console.ForegroundColor = ConsoleColor.DarkRed;

        Console.WriteLine(String.Format(STRING_ALIGNMENT1, "IDTABLEAU", "TITRETABLEAU", "SOUSTITRE", "IDTABLEAU"));
        Console.WriteLine(new string('-', 160));

        Console.ForegroundColor = ConsoleColor.Gray;

        foreach (DTO_Tableau tab in allTab)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(String.Format(
                STRING_ALIGNMENT1,
                tab.IdTableau.ToString() ?? "",
                tab.TitreTableau?.ToString() ?? "",
                tab.SousTitreTableau ?? string.Empty,
                tab.IdTableau.ToString() ?? ""
            ));

            Console.WriteLine(new string('-', 160));
            Console.ForegroundColor = ConsoleColor.Gray;

        }



=======
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
>>>>>>> ec2d882f7b2ce5fec3d5b24ddcb42ddbf52a6740
    }
}