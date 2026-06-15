namespace EF_API.Models;

public class Description
{
    public int? ArrayId { get; init; }
    public int? Position { get; init; }
    public int? Level { get; init; }
    public string? Mnemonic { get; init; } = string.Empty;
    public string? TextDescription { get; init; } = string.Empty;
    public string? FirstLineArray { get; init; } = string.Empty;
    public string? Line3LevelSpec { get; init; } = string.Empty;
    public string? Line4PchCont { get; init; } = string.Empty;
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
        string firstLineArray,
        string line3LevelSpec,
        string line4PchCont,
        int variation,
        int @decimal,
        int note
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
}