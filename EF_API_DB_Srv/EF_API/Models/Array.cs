namespace EF_API.Models;

public class Array
{
    public int ?ArrayId { get; init; }
    public string ?Title { get; init; }
    public string ?SubTitle { get; init; }
    
    public Array() { }
    
    public Array(int arrayId, string title, string subTitle)
    {
        this.ArrayId = arrayId;
        this.Title = title;
        this.SubTitle = subTitle;
    }
}