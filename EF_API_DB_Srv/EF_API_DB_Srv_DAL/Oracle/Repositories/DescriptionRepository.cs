using Microsoft.EntityFrameworkCore;
using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_SRV_Entities;
using EF_API_DB_SRV_Entities.Interfaces;

namespace EF_API_DB_Srv_DAL.Oracle.Repositories;

public class DescriptionRepository : IManipulation, IDisposable
{
    private readonly DBContext _dbContext;

    public DescriptionRepository(DBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IEnumerable<T>? GetDatas<T>(int selection)
    {
        if (typeof(T) == typeof(DTO_Description))
        {
            var (min, max) = SelectionConstants.GetMinMaxFromSelection(selection);
            var result = _dbContext.Descriptions
                .AsNoTracking()
                .Where(d => d.ArrayId >= min && d.ArrayId <= max)
                .ToList();
            
            return (IEnumerable<T>)(object)result;
        }

        throw new NotSupportedException($"Type {typeof(T).Name} Unsupported Type {selection}.");
    }
    
    public void Dispose()
    {
        _dbContext.Dispose();
    }
}