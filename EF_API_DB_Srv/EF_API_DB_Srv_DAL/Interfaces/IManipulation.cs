namespace EF_API_DB_SRV_Entities.Interfaces;

public interface IManipulation
{
    IEnumerable<T>? GetDatas<T>(int selection);
}