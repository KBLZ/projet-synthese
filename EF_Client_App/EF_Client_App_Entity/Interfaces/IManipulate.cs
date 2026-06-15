namespace EF_Client_App_Entity.Interfaces;

public interface IManipulate
{
    IEnumerable<T> GetDatas<T>(string? parameter = null);

}