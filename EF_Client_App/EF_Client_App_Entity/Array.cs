namespace EF_Client_App_Entity;

public class Array
{
    public int ArrayId { get; init; }
    public string ?Title { get; init; }
    public string ?SubTitle { get; init; }
    public string FormattedId => ArrayId.ToString();

    public Array() { }
    
    public Array(int arrayId, string title, string subTitle)
    {
        this.ArrayId = arrayId;
        this.Title = title;
        this.SubTitle = subTitle;
    }

    public override string ToString()
    {
        return $"ArrayId: {this.ArrayId}, " +
               $"Title: {this.Title}, " +
               $"SubTitle: {this.SubTitle}";
    }
}