namespace PE_DAL.Oracle;

public record DTO_Tableau
{
    public int IdTableau { get; init; }
    public string TitreTableau { get; init; } = string.Empty;
    public string? SousTitreTableau { get; init; }
    
    public DTO_Tableau() { }
    
    public DTO_Tableau(int idTableau, string titreTableau, string sousTitreTableau)
    {
        this.IdTableau = idTableau;
        this.TitreTableau = titreTableau;
        this.SousTitreTableau = sousTitreTableau;
    }
}