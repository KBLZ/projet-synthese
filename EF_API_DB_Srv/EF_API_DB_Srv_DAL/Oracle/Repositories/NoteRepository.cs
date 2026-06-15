using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_SRV_Entities;
using EF_API_DB_SRV_Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EF_API_DB_Srv_DAL.Oracle.Repositories;

public class NoteRepository : IManipulation, IDisposable
{
    private readonly DBContext _dbContext;

    public NoteRepository(DBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    public IEnumerable<T>? GetDatas<T>(int selection)
    {
        if (typeof(T) == typeof(DTO_Note))
        {
            var (min, max) = SelectionConstants.GetMinMaxFromSelection(selection);
          var result = _dbContext.Notes
              .AsNoTracking()
              .Where(n => n.NoteId >= min && n.NoteId <= max)
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