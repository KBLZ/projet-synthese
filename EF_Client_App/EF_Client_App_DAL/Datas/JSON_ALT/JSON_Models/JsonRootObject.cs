using System.Text.Json.Serialization;

namespace EF_Client_App_DAL.JSON;

public record JsonRootObject(
    [property: JsonPropertyName("_name")] string Name,
    [property: JsonPropertyName("_pages")] JsonPage[] Pages
    );