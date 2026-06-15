using System.Text.Json;
using System.Text.Json.Serialization;

namespace EF_Client_App_DAL.JSON;

public record JsonObject(
    [property: JsonPropertyName("_data_type")] string DataType,
    [property: JsonPropertyName("_name")] string Name,
    [property: JsonPropertyName("_obs_max")] JsonElement[]? ObsMax,
    [property: JsonPropertyName("_obs_min")] JsonElement[]? ObsMin,
    [property: JsonPropertyName("_type")] string Type,
    [property: JsonPropertyName("_values")] JsonElement[]? Values,
    [property: JsonPropertyName("_last_update")] JsonElement[]? LastUpdate,
    [property: JsonPropertyName("Convert_hilo")] string? ConvertHilo,
    [property: JsonPropertyName("Convert_lohi")] string? ConvertLohi
    );