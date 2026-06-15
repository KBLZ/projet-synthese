using System.Text.Json;
using System.Linq;
using EF_Client_App_DAL.JSON.DTO;
using EF_Client_App_Entity.Interfaces;
using EF_Client_App_Entity.Enums;
using EF_Client_App_Entity.Records;

namespace EF_Client_App_DAL.JSON;

public class Manipulation : IManipulate
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ForcastObjectDTO[] LoadForcastsObjects(string jsonFileName)
    {
        var baseDir = AppContext.BaseDirectory;
        var rootSolution = Path.GetFullPath(
            Path.Combine(baseDir, "..", "..", "..", ".."));

        var path = Path.Combine(
            rootSolution,
            "Configuration",
            "Secrets",
            jsonFileName);

        var json = File.ReadAllText(path);

        var root = JsonSerializer.Deserialize<JsonRootObject>(json, _options)
                   ?? throw new InvalidOperationException("JSON invalide ou vide.");

        Console.WriteLine($"Name racine: {root.Name ?? "<null>"}");
        Console.WriteLine($"Pages: {root.Pages.Length}");

        var jsonObjects = root.Pages
            .SelectMany(p => p.Objects ?? throw new InvalidOperationException("Page.Objects est null"))
            .Select(o => o ?? throw new InvalidOperationException("Object null détecté"))
            .Where(o =>
            {
                if (o.Values == null)
                    throw new InvalidOperationException("Values est null");

                if (o.LastUpdate == null)
                    throw new InvalidOperationException("LastUpdate est null");

                return true;
            })
            .ToArray();

        Console.WriteLine($"Total Objects JSON: {jsonObjects.Length}");

        var dtos = jsonObjects
            .Select(JsonObjectMapper.ConvertirEnDTO)
            .ToArray();

        return dtos;
    }

    public static ConfigurationImport? DeterminerConfigurationDepuisNomFichier(string nomFichier)
    {
        var nom = Path.GetFileNameWithoutExtension(nomFichier).ToLowerInvariant();

        DataTypes? dataType = null;
        GovernmentLevel? governmentLevel = null;

        if (nom.Contains("prev"))
            dataType = DataTypes.Forcast;
        else if (nom.Contains("comp"))
            dataType = DataTypes.Comparison;

        if (nom.Contains("can"))
            governmentLevel = GovernmentLevel.Federal;
        else if (nom.Contains("que"))
            governmentLevel = GovernmentLevel.Provincial;

        if (dataType is null || governmentLevel is null)
            return null;

        return new ConfigurationImport(dataType.Value, governmentLevel.Value);
    }

    public ForcastObjectDTO[] ChargerTousLesPrev()
    {
        var baseDir = AppContext.BaseDirectory;
        var rootSolution = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."));
        var folder = Path.Combine(rootSolution, "Configuration", "Secrets");

        var prevFiles = Directory.GetFiles(folder, "prev*.json");

        var all = new List<ForcastObjectDTO>();

        foreach (var path in prevFiles)
        {
            var name = Path.GetFileName(path);
            var objets = LoadForcastsObjects(name);
            all.AddRange(objets);
        }

        return all.ToArray();
    }

    public IEnumerable<T> GetDatas<T>(string? parameter = null)
    {
        throw new NotImplementedException();
    }
}