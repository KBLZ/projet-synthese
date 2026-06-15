namespace EF_Client_App_DAL;

public record NoteDTO
{
    public int NoteId { get; init; }
    public string NoteText { get; init; } = string.Empty;

    public NoteDTO()
    {
        ;
    }

    public NoteDTO(int noteId, string noteText)
    {
        NoteId = noteId;
        NoteText = noteText;
    }

    public override string ToString()
    {
        return $"NoteId: {this.NoteId}, " +
               $"NoteText: {this.NoteText}";
    }
}