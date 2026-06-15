using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_SRV_Entities.Interfaces;
using EF_API_DB_SRV_Entities;
using Microsoft.EntityFrameworkCore;

namespace EF_API_DB_Srv_DAL.Oracle.Repositories;

public class ArrayRepository : IManipulation, IDisposable
{
    private readonly DBContext _dbContext;

    public ArrayRepository(DBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IEnumerable<T>? GetDatas<T>(int selection)
    {
        if (typeof(T) == typeof(DTO_Array))
        {
            var (min, max) = SelectionConstants.GetMinMaxFromSelection(selection);
            var result = _dbContext.Arrays
                .AsNoTracking()
                .Where(a => a.ArrayId >= min && a.ArrayId <= max)
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