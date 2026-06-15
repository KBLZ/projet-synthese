namespace EF_API.Models;

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
}