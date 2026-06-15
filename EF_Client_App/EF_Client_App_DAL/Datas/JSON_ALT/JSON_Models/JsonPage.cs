using System.Text.Json;
using System.Text.Json.Serialization;

namespace EF_Client_App_DAL.JSON;

public record JsonPage(
    [property: JsonPropertyName("_frequency")] string Frequency,
    [property: JsonPropertyName("_last_update")] JsonElement[] LastUpdate,
    [property: JsonPropertyName("_name")] string Name,
    [property: JsonPropertyName("_obj_count")] int ObjCount,
    [property: JsonPropertyName("_objects")] JsonObject[] Objects,
    [property: JsonPropertyName("_obs_count")] int ObsCount
    );