namespace PE_DAL.Oracle;

public record DTO_Description
{
    public int ?IdTableau { get; init; }
    public int ?Position { get; init; }
    public int ?Niveau { get; init; }
    public string ?Mnemonic { get; init; } = string.Empty;
    public string ?DescriptionTexte { get; init; } = string.Empty;
    public string ?Ligne1Tableau { get; init; } = string.Empty;
    public string ?Ligne3NiveauSpec { get; init; } = string.Empty;
    public string ?Ligne4PchCont { get; init; } = string.Empty;
    public int ?Variation { get; init; }
    public int ?Decimale { get; init; }
    public int ?Note { get; init; }

    public DTO_Description() { }

    // Virtual to allow EF proxy if needed, though not strictly required for this setup
    public virtual DTO_Tableau? Tableau { get; init; }
    public virtual DTO_Note? NoteNavigation { get; init; }

    public DTO_Description(
        int idTableau,
        int position,
        int niveau,
        string mnemonic,
        string descriptionTexte,
        string ligne1Tableau,
        string ligne3NiveauSpec,
        string ligne4PchCont,
        int variation,
        int decimale,
        int note
    )
    {
        IdTableau = idTableau;
        Position = position;
        Niveau = niveau;
        Mnemonic = mnemonic;
        DescriptionTexte = descriptionTexte;
        Ligne1Tableau = ligne1Tableau;
        Ligne3NiveauSpec = ligne3NiveauSpec;
        Ligne4PchCont = ligne4PchCont;
        Variation = variation;
        Decimale = decimale;
        Note = note;
    }

}