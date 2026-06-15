using System.Reflection.Metadata;
using Entity = EF_Client_App_Entity;
namespace EF_Client_App_DAL;

public record DescriptionDTO
{
    public int ArrayId { get; init; }
    public int Position { get; init; }
    public int Level { get; init; }
    public string? Mnemonic { get; init; } = string.Empty;
    public string? TextDescription { get; init; } = string.Empty;
    public string? FirstLineArray { get; init; } = string.Empty;
    public string? Line3LevelSpec { get; init; } = string.Empty;
    public string? Line4PchCont { get; init; } = string.Empty;
    public int Variation { get; init; }
    public int Decimal { get; init; }
    public int? Note { get; init; }

    public DescriptionDTO()
    {
        ;
    }

    public DescriptionDTO(
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

    public Entity.Description ToEntity()
    {
        List<Entity.Serie> firstLineArrayList = new List<Entity.Serie>();
        Entity.Serie firstLineArray = new Entity.Serie();
        firstLineArray.ID = this.FirstLineArray;
        firstLineArrayList.Add(firstLineArray);

        List<Entity.Serie> line3LevelSpecList = new List<Entity.Serie>();
        Entity.Serie line3LevelSpec = new Entity.Serie();
        line3LevelSpec.ID = this.Line3LevelSpec;
        line3LevelSpecList.Add(line3LevelSpec);


        List<Entity.Serie> line4PchContList = new List<Entity.Serie>();
        Entity.Serie line4PchCont = new Entity.Serie();
        line4PchCont.ID = this.Line4PchCont;
        line4PchContList.Add(line3LevelSpec);


        return new Entity.Description(
            this.ArrayId,
            this.Position,
            this.Level,
            this.Mnemonic,
            this.TextDescription,
            firstLineArrayList,
            line3LevelSpecList,
            line4PchContList,
            this.Variation,
            this.Decimal,
            this.Note
        );
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