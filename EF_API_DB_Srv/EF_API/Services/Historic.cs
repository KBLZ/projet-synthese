using EF_API.Mapping;
using EF_API.Models;
using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_Srv_DAL.Oracle.Repositories;

namespace EF_API.Services;

public class Historic
{
    private readonly HistoricRepository _repository;
    private readonly HistoricMapper _mapper;

    public Historic(HistoricRepository repository)
    {
        _repository = repository;
        _mapper = new HistoricMapper();
    }

    public Models.Historic? GetByKey(string userId, string prnSelection)
    {
        var dto = _repository.GetByKey(userId, prnSelection);
        return dto == null ? null : _mapper.ToModel(dto);
    }
    
    public Models.Historic Save(Models.Historic model)
    {
        var dto = _mapper.ToDTO(model);
        var existing = _repository.GetByKey(dto.UserId, dto.PRN_Selection);

        if (existing == null)
            _repository.Create(dto);
        else
            _repository.Update(dto);

        return model;
    }
    
    public void Update(Models.Historic model)
    {
        var dto = _mapper.ToDTO(model);
        _repository.Update(dto);
    }
}