using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Interfaces;
using EF_API_DB_Srv_DAL.Oracle.DTO;

public class HistoricRepository : IHistoric, IDisposable
{
    private readonly DBContext _dbContext;

    public HistoricRepository(DBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public DTO_Historic? GetByKey(string userId, string prnSelection)
    {
        return _dbContext.Historics
            .FirstOrDefault(h => h.UserId == userId && h.PRN_Selection == prnSelection);
    }

    public void Update(DTO_Historic entity)
    {
        _dbContext.Historics.Update(entity);
        _dbContext.SaveChanges();
    }

    public DTO_Historic Create(DTO_Historic entity)
    {
        _dbContext.Historics.Add(entity);
        _dbContext.SaveChanges();
        return entity;
    }

    public void Dispose() => _dbContext.Dispose();
}