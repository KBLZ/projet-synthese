using EF_API.Mapping;
using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_Srv_DAL.Oracle.Repositories;

namespace EF_API.Services;

public class Array
{
    private readonly ArrayRepository _repository;

    public Array(ArrayRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    
    public IEnumerable<Models.Array> GetFiltereds(int selection)
    {
        var dtoArrays = _repository.GetDatas<DTO_Array>(selection);
        if (dtoArrays == null) return Enumerable.Empty<Models.Array>();
        
        return dtoArrays.Select(a => new ArrayMapper().ToModel(a));
    }
}