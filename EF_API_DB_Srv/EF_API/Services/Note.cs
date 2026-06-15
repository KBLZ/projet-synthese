using EF_API.Mapping;
using EF_API_DB_Srv_DAL.Oracle.Context;
using EF_API_DB_Srv_DAL.Oracle.DTO;
using EF_API_DB_Srv_DAL.Oracle.Repositories;

namespace EF_API.Services;

public class Note
{
    private readonly NoteRepository _repository;

    public Note(NoteRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IEnumerable<Models.Note> GetFiltereds(int selection)
    {
        var dtoNote = _repository.GetDatas<DTO_Note>(selection);
        if (dtoNote == null) return Enumerable.Empty<Models.Note>();
        
        return dtoNote.Select(a => new NoteMapper().ToModel(a));
    }
}