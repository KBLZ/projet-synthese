namespace EF_Client_App_Entity;

public class Note
{
    public int NoteId { get; init; }
    public string NoteText { get; init; } = string.Empty;

    public Note()
    {
        ;
    }

    public Note(int noteId, string noteText)
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