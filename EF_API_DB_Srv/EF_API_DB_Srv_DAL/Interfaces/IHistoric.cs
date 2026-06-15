using EF_API_DB_Srv_DAL.Oracle.DTO;

namespace EF_API_DB_Srv_DAL.Interfaces;

public interface IHistoric
{
    DTO_Historic? GetByKey(string userId, string prnSelection);
    DTO_Historic Create(DTO_Historic entity);
    void Update(DTO_Historic entity);
}