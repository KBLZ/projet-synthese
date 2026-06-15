using EF_API_DB_SRV_Entities.Interfaces;

namespace EF_API_DB_Srv_DAL.Oracle.DTO;

public record DTO_Note 
{
    public int NoteId { get; init; }
    public string NoteText { get; init; } = string.Empty;


    public DTO_Note()
    {
    }

    public DTO_Note(int noteId, string textNoteText)
    {
        this.NoteId = noteId;
        this.NoteText = textNoteText;
    }
}