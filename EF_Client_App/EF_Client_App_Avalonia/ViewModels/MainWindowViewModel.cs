using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EF_Client_App_Entity;
using EF_Client_UI_Avalonia;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Historic = EF_Client_App_Entity.Historic;

namespace AvaloniaApplication1.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _typeRecherche = "Prévisions économiques";

        [ObservableProperty]
        private string _tableau = Environment.UserName;

        [ObservableProperty]
        private string _banque1 = "";

        [ObservableProperty]
        private string _banque2 = "";

        [ObservableProperty]
        private object? _pageCourante;

        [ObservableProperty]
        private bool _isAccueil = true;

        [ObservableProperty]
        private Bitmap? _photoUtilisateur;

        [ObservableProperty]
        private bool _filtreTableau = false;

        [ObservableProperty]
        private bool _filtreDescription = false;

        [ObservableProperty]
        private bool _filtreMnemonique = false;

        [ObservableProperty]
        private string _motsCles = "";

        [ObservableProperty]
        private string _anneeDebut = (DateTime.Now.Year - 10).ToString();

        [ObservableProperty]
        private string _anneeFin = (DateTime.Now.Year + 20).ToString();

        [ObservableProperty]
        private string _trimestreDebut = "I";

        [ObservableProperty]
        private string _trimestreFin = "IV";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(EstModeTrimestriel))]
        [NotifyPropertyChangedFor(nameof(EstModeAnnuel))]
        [NotifyPropertyChangedFor(nameof(EstModeTrimestrielAnnuel))]
        private ModePeriode _modePeriode = ModePeriode.Trimestriel;

        // Propriétés booléennes pour le style « bouton actif » en XAML
        public bool EstModeTrimestriel       => ModePeriode == ModePeriode.Trimestriel;
        public bool EstModeAnnuel            => ModePeriode == ModePeriode.Annuel;
        public bool EstModeTrimestrielAnnuel => ModePeriode == ModePeriode.TrimestrielAnnuel;

        
        // PRN métier courante : PREVCAN, COMPCAN, PREVQUE, COMPQUE
        [ObservableProperty]
        private string? currentPrnSelection;

        // CONFIGURATION : Changé en ObservableCollection pour que le XAML réagisse aux modifications de la sidebar
        public ObservableCollection<BoutonTableauDummy> ListeDeTest { get; } = new();

        [ObservableProperty]
        private EF_Client_App_Entity.Array? _selectedArray;

        private bool _estRegionQuebec;  // !!!

        private bool _estModeComparaison; // AJOUTÉ : permet de mémoriser définitivement si on compare

        public ObservableCollection<EF_Client_App_Entity.Array> Arrays { get; } = new();

        private readonly IDataService _dataService;
        private readonly IServiceProvider _serviceProvider;
        public IDataService DataService => _dataService;

        public MainWindowViewModel(IDataService dataService, IServiceProvider serviceProvider)
        {
            _dataService = dataService;
            _serviceProvider = serviceProvider;

            PageCourante = new AccueilViewModel(this);
            ChargerIconeSilhouette();

            // CHARGEMENT INITIAL : On lance le chargement global dès l'ouverture de l'application
            // sans bloquer le thread principal (UI)
            Task.Run(async () =>
            {
                try
                {
                    await _dataService.LoadAllDataAsync();
                    System.Diagnostics.Debug.WriteLine($"[INIT DÉMARRAGE] Données chargées avec succès. Tableaux : {_dataService.Arrays.Count}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ERREUR INIT] {ex.Message}");
                }
            });
        }

        // Cette méthode s'exécute AUTOMATIQUEMENT dès que PageCourante change
        partial void OnPageCouranteChanged(object? oldValue, object? newValue)
        {
            if (newValue is ParcourirViewModel parcourirVM)
            {
                // On configure la région directement selon le type de section de la page
                _estRegionQuebec = (parcourirVM.Section == TypeSection.PrevisionQuebec ||
                                    parcourirVM.Section == TypeSection.ComparaisonQuebec);

                RafraichirSidebar();
            }
        }

        public void ChargerTableauxPourSection(TypeSection section)
        {
            // CONFIGURATION : On capture de façon permanente le choix de l'accueil
            _estRegionQuebec = (section == TypeSection.PrevisionQuebec || section == TypeSection.ComparaisonQuebec);
            _estModeComparaison = (section == TypeSection.ComparaisonQuebec || section == TypeSection.ComparaisonCanada);

            RafraichirSidebar();
        }

        public void RafraichirSidebar()
        {
            Arrays.Clear();
            ListeDeTest.Clear();

            bool estComparaison = false;

            // 1. Détection de la région et du mode selon la page courante
            if (PageCourante is ParcourirViewModel parcourirVM)
            {
                estComparaison = parcourirVM.EstComparaison;

                // Détection initiale par la section choisie sur l'accueil
                _estRegionQuebec = (parcourirVM.Section == TypeSection.PrevisionQuebec ||
                                    parcourirVM.Section == TypeSection.ComparaisonQuebec);

                // Ajustement si un fichier spécifique change la donne
                string nomFichier = (estComparaison ? parcourirVM.FichierGaucheSelectionne : parcourirVM.FichierSelectionne) ?? "";
                if (!string.IsNullOrEmpty(nomFichier))
                {
                    _estRegionQuebec = nomFichier.Contains("q", StringComparison.OrdinalIgnoreCase);
                }
            }
            else if (PageCourante is ResultatViewModel)
            {
                estComparaison = TypeRecherche != null && TypeRecherche.Contains("Comparaison", StringComparison.OrdinalIgnoreCase);

                if (TypeRecherche != null && TypeRecherche.Contains("Québec", StringComparison.OrdinalIgnoreCase))
                {
                    _estRegionQuebec = true;
                }
            }

            // 2. Filtrage et construction de l'affichage basé sur les vrais IDs de la BD
            //System.Diagnostics.Debug.WriteLine($"[DEBUG] Nombre de tableaux en BD: {_dataService.Arrays?.Count ?? 0}");
            //System.Diagnostics.Debug.WriteLine($"[DEBUG] Région Québec ? {_estRegionQuebec}");

            foreach (var array in _dataService.Arrays)
            {
                bool correspondALaRegion = false;

                if (_estRegionQuebec)
                {
                    // Québec Simple (201 à 222) OU Québec Comparaison (401 à 422)
                    if ((array.ArrayId >= 201 && array.ArrayId <= 222) || (array.ArrayId >= 401 && array.ArrayId <= 422))
                    {
                        correspondALaRegion = true;
                    }
                }
                else
                {
                    // Canada Simple (101 à 130) OU Canada Comparaison (301 à 330)
                    if ((array.ArrayId >= 101 && array.ArrayId <= 130) || (array.ArrayId >= 301 && array.ArrayId <= 330))
                    {
                        correspondALaRegion = true;
                    }
                }

                if (correspondALaRegion)
                {
                    // On conserve l'objet complet dans Arrays pour votre logique de traitement arrière-plan
                    Arrays.Add(array);

                    // Construction du numéro visuel propre (Ex: "101/301" ou "201/401")
                    string numeroAAfficher;
                    if (estComparaison)
                    {
                        if (_estRegionQuebec)
                        {
                            // Si on est sur un ID 400 (comparaison), on extrait la base 200
                            int baseId = (array.ArrayId >= 401) ? array.ArrayId - 200 : array.ArrayId;
                            numeroAAfficher = $"{baseId}/{baseId + 200}";
                        }
                        else
                        {
                            // Si on est sur un ID 300 (comparaison), on extrait la base 100
                            int baseId = (array.ArrayId >= 301) ? array.ArrayId - 200 : array.ArrayId;
                            numeroAAfficher = $"{baseId}/{baseId + 200}";
                        }
                    }
                    else
                    {
                        numeroAAfficher = array.ArrayId.ToString();
                    }

                    // Ajout du bouton à votre liste d'affichage
                    ListeDeTest.Add(new BoutonTableauDummy
                    {
                        Numero = numeroAAfficher,
                        Titre = array.Title ?? string.Empty,
                        OriginalArrayId = array.ArrayId
                    });
                }
            }
        }

        public void AnnulerFiltres()
        {
            FiltreTableau    = false;
            FiltreDescription = false;
            FiltreMnemonique = false;
            MotsCles         = "";
            AnneeDebut       = (DateTime.Now.Year - 10).ToString();
            AnneeFin         = (DateTime.Now.Year + 20).ToString();
            TrimestreDebut   = "I";
            TrimestreFin     = "IV";
            ModePeriode      = ModePeriode.Trimestriel;
            RafraichirResultat();
        }

        // -------------------------------------------------------
        // Commandes des boutons de mode (Trimestriel / Annuel / Mixte)
        // -------------------------------------------------------
        [RelayCommand]
        public void SetModeTrimestriel()
        {
            ModePeriode = ModePeriode.Trimestriel;
            RafraichirResultat();
        }

        [RelayCommand]
        public void SetModeAnnuel()
        {
            ModePeriode = ModePeriode.Annuel;
            RafraichirResultat();
        }

        [RelayCommand]
        public void SetModeTrimestrielAnnuel()
        {
            ModePeriode = ModePeriode.TrimestrielAnnuel;
            RafraichirResultat();
        }

        // -------------------------------------------------------
        // Commande du bouton « Rechercher avec les filtres »
        // -------------------------------------------------------
        [RelayCommand]
        public void Rechercher()
        {
            RafraichirResultat();
        }

        private void RafraichirResultat()
        {
            if (PageCourante is ResultatViewModel resultatVM)
            {
                resultatVM.RafraichirSelectionApresClic();
            }
        }
        
        // MainWindowViewModel.cs
        public Historic? BuildHistoric()
        {
            // On ne sauvegarde que si l'utilisateur est sur un vrai écran de travail
            if (IsAccueil) return null;

            int.TryParse(AnneeDebut, out int anneeDebut);
            int trimestreDebut = TrimestreDebut switch
            {
                "I" => 1, "II" => 2, "III" => 3, "IV" => 4, _ => 1
            };

            return new Historic(
                userId:         Environment.UserName,
                prnSelection:   CurrentPrnSelection ?? "",
                urlPool1:       Banque1,
                urlPool2:       Banque2,
                startedYear:    anneeDebut > 0 ? anneeDebut : DateTime.Now.Year - 10,
                startedQuarter: trimestreDebut,
                indexTitleTab:  0,   // à adapter selon ton UI
                displayMode:    0    // à adapter selon ton mode d'affichage
            );
        }
        
        [RelayCommand]
        public void SelectArray(BoutonTableauDummy bouton)
        {
            var original = _dataService.Arrays.FirstOrDefault(a => a.ArrayId == bouton.OriginalArrayId);
            if (original != null)
            {
                SelectedArray = original;

                // AJOUT : Si la page courante est l'écran de résultats, on lui demande de rafraîchir ses données !
                if (PageCourante is ResultatViewModel resultatVM)
                {
                    resultatVM.RafraichirSelectionApresClic();
                }
            }
        }

        private void ChargerIconeSilhouette()
        {
            try
            {
                var uri = new Uri("avares://EF_Client_UI_Avalonia/Assets/silhouette.png");
                using var stream = AssetLoader.Open(uri);
                PhotoUtilisateur = new Bitmap(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Impossible de charger l'icône de silhouette : {ex.Message}");
            }
        }
    }

    // Code test pour les boutons de tableau sur le côté (tableau)
    // Ce code est uniquement pour ajuster le visuel
    public class BoutonTableauDummy
    {
        public string Numero { get; set; } = string.Empty;
        public string Titre { get; set; } = string.Empty;
        public int OriginalArrayId { get; set; } // CONFIGURATION : Ajouté pour lier l'affichage à la BD
    }

    /// <summary>
    /// Mode d'affichage des données dans le DataGrid.
    /// Trimestriel   : colonnes trimestrielles, séries Q uniquement.
    /// Annuel        : colonnes annuelles, séries A uniquement.
    /// TrimestrielAnnuel : colonnes trimestrielles, séries Q ET A affichées.
    /// </summary>
    public enum ModePeriode
    {
        Trimestriel,
        Annuel,
        TrimestrielAnnuel
    }
}