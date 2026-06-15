using EF_API_DB_SRV_Entities.Interfaces;

namespace EF_API_DB_Srv_DAL.Oracle.DTO;

public class DTO_Historic 
{
    public string UserId { get; init; } = string.Empty;
    public string PRN_Selection { get; init; } = string.Empty;
    public string? UrlPool1 { get; set; } = string.Empty;
    public string? UrlPool2 { get; set; } = string.Empty;

    public int StartedYear { get; set; }
    public int StartedQuarter { get; set; }
    public int IndexTitleTab { get; set; }
    public int DisplayMode { get; set; }


    public DTO_Historic() { }

    public DTO_Historic(
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