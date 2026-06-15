using EF_API.Mapping;
using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_Srv_DAL.Oracle.Repositories;

namespace EF_API.Services;

public class Description
{
    private readonly DescriptionRepository _repository;

    public Description(DescriptionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IEnumerable<Models.Description> GetFiltereds(int selection)
    {
        var dtoDescription = _repository.GetDatas<DTO_Description>(selection);
        if (dtoDescription == null) return Enumerable.Empty<Models.Description>();

        return dtoDescription.Select(d => new DescriptionMapper().ToModel(d));
    }
}