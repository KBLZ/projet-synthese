
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using PE_DAL;
using PE_DAL.Oracle.Repositories;

// Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string connectionString = configuration.GetConnectionString("OracleDb") ?? "";

Console.WriteLine("Tentative de connexion à la base de données Oracle...");

try
{
    var manipulationOracle = new Manipulation_Oracle(connectionString);
    var tableauRepo = new DTO_TableauRepository(manipulationOracle);

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