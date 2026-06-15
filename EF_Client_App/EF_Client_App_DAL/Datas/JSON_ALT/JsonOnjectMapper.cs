using System.Text.Json;
using EF_Client_App_DAL.JSON.DTO;

namespace EF_Client_App_DAL.JSON;

public static class JsonObjectMapper
{
    public static ForcastObjectDTO ConvertirEnDTO(JsonObject jsonObject)
    {
        var obsMax = ConvertirObservationPoint(jsonObject.ObsMax, "ObsMax");
        var obsMin = ConvertirObservationPoint(jsonObject.ObsMin, "ObsMin");
        var lastUpdate = ConvertirUpdate(jsonObject.LastUpdate, "LastUpdate");
        var values = ConvertirValeurs(jsonObject.Values);

        return new ForcastObjectDTO(
            jsonObject.Name,
            jsonObject.DataType,
            jsonObject.Type,
            obsMax,
            obsMin,
            lastUpdate,
            values,
            jsonObject.ConvertHilo,
            jsonObject.ConvertLohi
        );
    }

    private static ObservationPointDTO ConvertirObservationPoint(JsonElement[]? source, string fieldName)
    {
        if (source is null || source.Length < 2)
            throw new InvalidOperationException($"{fieldName} doit contenir au moins 2 éléments.");

        var index = LireDouble(source[0], $"{fieldName}[0]");
        var date = LireDateOnly(source[1], $"{fieldName}[1]");

        return new ObservationPointDTO(index, date);
    }

    private static UpdateDTO ConvertirUpdate(JsonElement[]? source, string fieldName)
    {
        if (source is null || source.Length < 2)
            throw new InvalidOperationException($"{fieldName} doit contenir au moins 2 éléments.");

        var index = LireDouble(source[0], $"{fieldName}[0]");
        var date = LireDateTime(source[1], $"{fieldName}[1]");

        return new UpdateDTO(index, date);
    }

    private static double[] ConvertirValeurs(JsonElement[]? source)
    {
        if (source is null)
            throw new InvalidOperationException("Values est null depuis l'API.");

        return source
            .Select((element, index) => LireDouble(element, $"Values[{index}]"))
            .ToArray();
    }

    private static double LireDouble(JsonElement element, string fieldName)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.String when double.TryParse(element.GetString(), out var value) => value,
            _ => throw new InvalidOperationException($"{fieldName} n'est pas un nombre valide.")
        };
    }

    private static DateOnly LireDateOnly(JsonElement element, string fieldName)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String when DateOnly.TryParse(element.GetString(), out var value) => value,
            JsonValueKind.String when DateTime.TryParse(element.GetString(), out var dateTime) => DateOnly.FromDateTime(dateTime),
            _ => throw new InvalidOperationException($"{fieldName} n'est pas une date valide.")
        };
    }

    private static DateTime LireDateTime(JsonElement element, string fieldName)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetDateTime(),
            _ => throw new InvalidOperationException($"{fieldName} n'est pas une date/heure valide.")
        };
    }
}