using EF_API_DB_Srv_DAL.Oracle.DTO;
using Model = EF_API.Models;

namespace EF_API.Mapping;

public class NoteMapper
{
    public Model.Note ToModel(DTO_Note p_DTONote)
    {
        if (p_DTONote == null) return new Model.Note();

        return new Model.Note
        {
            NoteId = p_DTONote.NoteId,
            NoteText = p_DTONote.NoteText
        };
    }

    public DTO_Note ToDTO(Model.Note p_noteModel)
    {
        if (p_noteModel == null) return new DTO_Note();

        return new DTO_Note
        {
            NoteId = p_noteModel.NoteId,
            NoteText = p_noteModel.NoteText
        };
    }
}