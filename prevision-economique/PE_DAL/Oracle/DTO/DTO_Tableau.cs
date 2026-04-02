namespace PE_DAL.Oracle;

public record DTO_Tableau
{
<<<<<<< HEAD
    public int ?IdTableau { get; init; }
    public string ?TitreTableau { get; init; }
    public string ?SousTitreTableau { get; init; }
=======
    public int IdTableau { get; init; }
    public string TitreTableau { get; init; } = string.Empty;
    public string? SousTitreTableau { get; init; }
>>>>>>> ec2d882f7b2ce5fec3d5b24ddcb42ddbf52a6740
    
    public DTO_Tableau() { }
    
    public DTO_Tableau(int idTableau, string titreTableau, string sousTitreTableau)
    {
        this.IdTableau = idTableau;
        this.TitreTableau = titreTableau;
        this.SousTitreTableau = sousTitreTableau;
    }
}