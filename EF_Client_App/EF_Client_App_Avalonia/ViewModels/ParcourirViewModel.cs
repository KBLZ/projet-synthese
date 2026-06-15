using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AvaloniaApplication1.ViewModels
{
    public enum TypeSection
    {
        PrevisionQuebec,
        PrevisionCanada,
        ComparaisonQuebec,
        ComparaisonCanada
    }

    public partial class ParcourirViewModel : ViewModelBase
    {
        private static string CheminBase => App.CheminJson;

        // ====================================================================
        // AJOUT : On déclare le champ privé pour conserver l'accès à MainWindowViewModel
        // ====================================================================
        private readonly MainWindowViewModel _mainVM;

        [ObservableProperty]
        private TypeSection _section;

        [ObservableProperty]
        private string _titreSection = string.Empty;

        // --- Mode simple (Prevision) ---
        public List<string> FichiersAutorises { get; private set; } = new();

        [ObservableProperty]
        private string? _fichierSelectionne;

        [ObservableProperty]
        private string? _cheminFichierSelectionne;

        // --- Mode comparaison (2 colonnes) ---
        [ObservableProperty]
        private bool _estComparaison;

        // Colonne gauche
        public List<string> FichiersAutorisesGauche { get; private set; } = new();

        [ObservableProperty]
        private string? _fichierGaucheSelectionne;

        [ObservableProperty]
        private string? _cheminGaucheSelectionne;

        // Colonne droite
        public List<string> FichiersAutorisesDroite { get; private set; } = new();

        [ObservableProperty]
        private string? _fichierDroiteSelectionne;

        [ObservableProperty]
        private string? _cheminDroiteSelectionne;

        // Champ année début
        [ObservableProperty]
        private string _anneeDebut = string.Empty;

        // Placeholder = année actuelle
        public string AnneePlaceholder => DateTime.Now.Year.ToString();

        // Méthode de validation
        public int ObtenirAnneeDebutValidee()
        {
            int anneeActuelle = DateTime.Now.Year;
            if (int.TryParse(AnneeDebut, out int annee) && annee >= 1900 && annee <= 2100)
                return annee;
            return anneeActuelle - 10;
        }

        public ParcourirViewModel(MainWindowViewModel mainVM, TypeSection type)
        {
            // ====================================================================
            // AJOUT : On assigne le paramètre au champ privé dès le départ
            // ====================================================================
            _mainVM = mainVM;

            _section = type;
            mainVM.IsAccueil = true;

            switch (type)
            {
                case TypeSection.PrevisionQuebec:
                    mainVM.TypeRecherche = "Prévisions du Québec";
                    TitreSection = "Prévision économique - Québec";
                    EstComparaison = false;
                    FichiersAutorises = new List<string> { "prevquewf1.json", "prevquewf2.json", "prevquewf3.json" };
                    break;

                case TypeSection.PrevisionCanada:
                    mainVM.TypeRecherche = "Prévisions du Canada";
                    TitreSection = "Prévision économique - Canada";
                    EstComparaison = false;
                    FichiersAutorises = new List<string> { "prevcanwf1.json", "prevcanwf2.json" };
                    break;

                case TypeSection.ComparaisonQuebec:
                    mainVM.TypeRecherche = "Comparaison des prévisions du Québec";
                    TitreSection = "Comparaison - Québec";
                    EstComparaison = true;
                    FichiersAutorisesGauche = new List<string> { "prevquewf1.json", "prevquewf2.json", "prevquewf3.json" };
                    FichiersAutorisesDroite = new List<string> { "compquewf1.json", "compquewf21.json", "compquewf23.json" };
                    break;

                case TypeSection.ComparaisonCanada:
                    mainVM.TypeRecherche = "Comparaison des prévisions du Canada";
                    TitreSection = "Comparaison - Canada";
                    EstComparaison = true;
                    FichiersAutorisesGauche = new List<string> { "prevcanwf1.json", "prevcanwf2.json" };
                    FichiersAutorisesDroite = new List<string> { "compcanwf1.json", "compcanwf12.json" };
                    break;
            }

            mainVM.ChargerTableauxPourSection(type);
        }

        public void SetFichierSelectionne(string cheminComplet)
        {
            CheminFichierSelectionne = cheminComplet;
            FichierSelectionne = System.IO.Path.GetFileName(cheminComplet);

            // Pour rafraîchir les boutons dès qu'un nouveau fichier est chargé :
            _mainVM.RafraichirSidebar();
        }

        public void SetFichierGauche(string cheminComplet)
        {
            CheminGaucheSelectionne = cheminComplet;
            FichierGaucheSelectionne = System.IO.Path.GetFileName(cheminComplet);

            // Pour rafraîchir les boutons dès qu'un nouveau fichier est chargé :
            _mainVM.RafraichirSidebar();
        }

        public void SetFichierDroite(string cheminComplet)
        {
            CheminDroiteSelectionne = cheminComplet;
            FichierDroiteSelectionne = System.IO.Path.GetFileName(cheminComplet);

            // Pour rafraîchir les boutons dès qu'un nouveau fichier est chargé :
            _mainVM.RafraichirSidebar();
        }
    }
}