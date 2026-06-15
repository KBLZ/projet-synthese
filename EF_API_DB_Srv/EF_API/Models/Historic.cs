using System.Text.Json.Serialization;

namespace EF_API.Models;

public class Historic
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("prn_Selection")]
    public string PRN_Selection { get; set; } = string.Empty;

    [JsonPropertyName("urlPool1")]
    public string? UrlPool1 { get; set; }

    [JsonPropertyName("urlPool2")]
    public string? UrlPool2 { get; set; }

    [JsonPropertyName("startedYear")]
    public int StartedYear { get; set; }

    [JsonPropertyName("startedQuarter")]
    public int StartedQuarter { get; set; }

    [JsonPropertyName("indexTitleTab")]
    public int IndexTitleTab { get; set; }

    [JsonPropertyName("displayMode")]
    public int DisplayMode { get; set; }

    public Historic()
    {
        ;
    }

    public Historic(
        string userId,
        string prnSelection,
        string urlPool1,
        string urlPool2,
        int startedYear,
        int startedQuarter,
        int indexTitleTab,
        int displayMode
    )
    {
        UserId = userId;
        PRN_Selection = prnSelection;
        UrlPool1 = urlPool1;
        UrlPool2 = urlPool2;
        StartedYear = startedYear;
        StartedQuarter = startedQuarter;
        IndexTitleTab = indexTitleTab;
        DisplayMode = displayMode;
    }
}