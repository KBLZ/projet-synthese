namespace PE_DAL.Oracle;

public record DTO_Note
{
    public int IdNote { get; init; }
    public string TexteNote { get; init; }
    public DTO_Note() { }
    public DTO_Note(int idNote, string textNote)
    {
        this.IdNote = idNote;
        this.TexteNote = textNote;
    }
}