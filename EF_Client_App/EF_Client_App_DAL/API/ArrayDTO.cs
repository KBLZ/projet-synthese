using Entity = EF_Client_App_Entity;
namespace EF_Client_App_DAL;

public record ArrayDTO
{
    public int ArrayId { get; init; }
    public string ?Title { get; init; }
    public string ?SubTitle { get; init; }
    
    public ArrayDTO() { }
    
    public ArrayDTO(int arrayId, string title, string subTitle)
    {
        this.ArrayId = arrayId;
        this.Title = title;
        this.SubTitle = subTitle;
    }

    public Entity.Array ToEntity()
    {
        return new Entity.Array(
            this.ArrayId,
            this.Title,
            this.SubTitle
        );
    }

    public override string ToString()
    {
        return $"ArrayId: {this.ArrayId}, " +
               $"Title: {this.Title}, " +
               $"SubTitle: {this.SubTitle}";
    }
}