using EF_API_DB_SRV_Entities.Interfaces;

namespace EF_API_DB_Srv_DAL.Oracle.DTO;

public record DTO_Array 
{
    public int ?ArrayId { get; init; }
    public int Id => ArrayId ?? 0;
    public string ?Title { get; init; }
    public string ?SubTitle { get; init; }

    public DTO_Array() { }


    public DTO_Array(int arrayId, string title, string subTitle)
    {
        this.ArrayId = arrayId;
        this.Title = title;
        this.SubTitle = subTitle;
    }
}