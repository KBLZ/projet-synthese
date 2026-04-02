namespace PE_DAL.Oracle;

public record DTO_Historique
{
    public string IdUtilisateur { get; init; } = string.Empty; 

    public string ChoixPRN { get; init; } = string.Empty;
    public string UrlBanque1 { get; init; } = string.Empty;
    public string UrlBanque2 { get; init; } = string.Empty;

    public int AnneeDebut { get; init; }
    public int TrimDebut { get; init; }
    public int IndexTitreTab { get; set; }
    public int ModeAffichage { get; set; }

    public DTO_Historique() { }

    public DTO_Historique(
        string idUtilisateur,
        string choixPRN,
        string urlBanque1,
        string urlBanque2,
        int anneeDebut,
        int trimDebut,
        int indexTitreTab,
        int modeAffichage
    )
    {
        this.IdUtilisateur = idUtilisateur;
        this.ChoixPRN = choixPRN;
        this.UrlBanque1 = urlBanque1;
        this.UrlBanque2 = urlBanque2;
        this.AnneeDebut = anneeDebut;
        this.TrimDebut = trimDebut;
        this.IndexTitreTab = indexTitreTab;
        this.ModeAffichage = modeAffichage;
    }
}