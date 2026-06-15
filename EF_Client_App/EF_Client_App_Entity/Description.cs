namespace EF_Client_App_Entity;

public class Description
{
    public int? ArrayId { get; init; }
    public int? Position { get; init; }
    public int? Level { get; init; }
    public string? Mnemonic { get; init; } = string.Empty;
    public string? TextDescription { get; init; } = string.Empty;
    public string FormattedDescription
    {
        get
        {
            if (Level == 1) return "  • " + TextDescription;
            if (Level == 2) return "    - " + TextDescription;
            return TextDescription ?? string.Empty;
        }
    }
    public List<Serie>? FirstLineArray { get; set; } = new List<Serie>();
    public List<Serie>? Line3LevelSpec { get; set; } = new List<Serie>();
    public List<Serie>? Line4PchCont { get; set; } = new List<Serie>();
    public int? Variation { get; init; }
    public int? Decimal { get; init; }
    public int? Note { get; init; }

    public Description()
    {
        ;
    }

    public Description(
        int arrayId,
        int position,
        int level,
        string mnemonic,
        string textDescription,
        List<Serie> firstLineArray,
        List<Serie> line3LevelSpec,
        List<Serie> line4PchCont,
        int variation,
        int @decimal,
        int? note
    )
    {
        ArrayId = arrayId;
        Position = position;
        Level = level;
        Mnemonic = mnemonic;
        TextDescription = textDescription;
        FirstLineArray = firstLineArray;
        Line3LevelSpec = line3LevelSpec;
        Line4PchCont = line4PchCont;
        Variation = variation;
        Decimal = @decimal;
        Note = note;
    }

    public override string ToString()
    {
        return $"ArrayId: {this.ArrayId}, " +
               $"Position: {this.Position}, " +
               $"Level: {this.Level}, " +
               $"Mnemonic: {this.Mnemonic}, " +
               $"TextDescription: {this.TextDescription}, " +
               $"FirstLineArray: {this.FirstLineArray}, " +
               $"Line3LevelSpec: {this.Line3LevelSpec}, " +
               $"Line4PchCont: {this.Line4PchCont}, " +
               $"Variation: {this.Variation}, " +
               $"Decimal: {this.Decimal}, " +
               $"Note: {this.Note}";
    }
}