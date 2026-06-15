namespace EF_Client_App_Entity;

public class Historic
{
    public string UserId { get; init; } = string.Empty;
    public string PRN_Selection { get; init; } = string.Empty;
    public string UrlPool1 { get; init; } = string.Empty;
    public string UrlPool2 { get; init; } = string.Empty;

    public int StartedYear { get; init; }
    public int StartedQuarter { get; init; }
    public int IndexTitleTab { get; set; }
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

    public override string ToString()
    {
        return $"UserId: {this.UserId}, " +
               $"PRN_Selection: {this.PRN_Selection}, " +
               $"UrlPool1: {this.UrlPool1}, " +
               $"UrlPool2: {this.UrlPool2}, " +
               $"StartedYear: {this.StartedYear}, " +
               $"StartedQuarter: {this.StartedQuarter}, " +
               $"IndexTitleTab: {this.IndexTitleTab}, " +
               $"DisplayMode: {this.DisplayMode}";
    }
}