using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using AvaloniaApplication1.ViewModels;
using EF_Client_UI_Avalonia;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Views;

public partial class ParcourirView : UserControl
{
    public ParcourirView()
    {
        InitializeComponent();
    }

    private async void btn_Parcourir_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not ParcourirViewModel modele) return;
        string? cheminFichier = await OuvrirExplorateur();
        if (cheminFichier != null)
            modele.SetFichierSelectionne(cheminFichier);
    }

    private async void btn_ParcourirGauche_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not ParcourirViewModel modele) return;
        string? cheminFichier = await OuvrirExplorateur();
        if (cheminFichier != null)
            modele.SetFichierGauche(cheminFichier);
    }

    private async void btn_ParcourirDroite_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not ParcourirViewModel modele) return;
        string? cheminFichier = await OuvrirExplorateur();
        if (cheminFichier != null)
            modele.SetFichierDroite(cheminFichier);
    }

    private async Task<string?> OuvrirExplorateur()
    {
        TopLevel? niveauSuperior = TopLevel.GetTopLevel(this);
        if (niveauSuperior == null) return null;

        IReadOnlyList<IStorageFile> fichiersCboisis = await niveauSuperior.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Choisissez un fichier",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Tous les fichiers permis") { Patterns = new[] { "*.json", "*.csv", "*.wf1" } },
                new FilePickerFileType("Fichiers JSON") { Patterns = new[] { "*.json" } },
                new FilePickerFileType("Fichiers CSV") { Patterns = new[] { "*.csv" } },
                new FilePickerFileType("Fichiers WF1 (EViews)") { Patterns = new[] { "*.wf1" } }
            }
        });

        if (fichiersCboisis.Count == 0) return null;
        return fichiersCboisis[0].Path.LocalPath;
    }

    private async void btn_Valider_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not ParcourirViewModel modele) return;

        MainWindowViewModel modelePrincipal = (MainWindowViewModel)this.VisualRoot?.DataContext;

        if (!modele.EstComparaison)
        {
            string nomFichier = Path.GetFileName(modele.CheminFichierSelectionne ?? "");
            if (!modele.FichiersAutorises.Contains(nomFichier))
            {
                await AfficherErreur($"Le fichier '{nomFichier}' n'est pas compatible avec cette section.\nFichiers acceptes : {string.Join(", ", modele.FichiersAutorises)}");
                return;
            }
            modelePrincipal.Banque1 = nomFichier;
            modelePrincipal.Banque2 = "";
        }
        else
        {
            string nomGauche = Path.GetFileName(modele.CheminGaucheSelectionne ?? "");
            string nomDroite = Path.GetFileName(modele.CheminDroiteSelectionne ?? "");

            if (!modele.FichiersAutorisesGauche.Contains(nomGauche))
            {
                await AfficherErreur($"Le fichier de prévision '{nomGauche}' n'est pas compatible.\nFichiers acceptes : {string.Join(", ", modele.FichiersAutorisesGauche)}");
                return;
            }
            if (!modele.FichiersAutorisesDroite.Contains(nomDroite))
            {
                await AfficherErreur($"Le fichier de comparaison '{nomDroite}' n'est pas compatible.\nFichiers acceptes : {string.Join(", ", modele.FichiersAutorisesDroite)}");
                return;
            }
            modelePrincipal.Banque1 = nomGauche;
            modelePrincipal.Banque2 = nomDroite;
        }

        // ================================================================
        // VALIDATION ANNÉE DÉBUT ET FIN PAR RAPPORT AU JSON
        // ================================================================
        int anneeActuelle = DateTime.Now.Year;
        string cheminPourValidation = modele.EstComparaison
            ? (modele.CheminGaucheSelectionne ?? "")
            : (modele.CheminFichierSelectionne ?? "");

        int anneeMinJson = ObtenirAnneeMinDepuisJson(cheminPourValidation);
        int anneeMaxJson = ObtenirAnneeMaxDepuisJson(cheminPourValidation);

        // --- Validation année début (entree dans Parcourir) ---
        bool anneeDebutValide = int.TryParse(modele.AnneeDebut, out int anneeDebutEntree)
                                && anneeDebutEntree >= anneeMinJson
                                && anneeDebutEntree <= anneeMaxJson;

        if (anneeDebutValide)
        {
            modelePrincipal.AnneeDebut = anneeDebutEntree.ToString();
        }
        else
        {
            if (!string.IsNullOrEmpty(modele.AnneeDebut))
            {
                await AfficherErreur($"L'année début '{modele.AnneeDebut}' n'existe pas dans le fichier.\n" +
                                     $"Plage disponible : {anneeMinJson} à {anneeMaxJson}.\n" +
                                     $"L'année de début sera ajustée à {anneeActuelle - 10}.");
            }
            modelePrincipal.AnneeDebut = (anneeActuelle - 10).ToString();
        }

        // --- Validation année fin (deja dans le footer du MainWindow) ---
        // Si l utilisateur n a pas encore modifie l annee fin → on met l annee max du JSON
        if (string.IsNullOrEmpty(modelePrincipal.AnneeFin))
        {
            modelePrincipal.AnneeFin = anneeMaxJson.ToString();
        }
        else
        {
            bool anneeFinValide = int.TryParse(modelePrincipal.AnneeFin, out int anneeFinEntree)
                                  && anneeFinEntree >= anneeMinJson
                                  && anneeFinEntree <= anneeMaxJson;

            if (!anneeFinValide)
            {
                await AfficherErreur($"L'année fin '{modelePrincipal.AnneeFin}' n'existe pas dans le fichier.\n" +
                                     $"Plage disponible : {anneeMinJson} à {anneeMaxJson}.\n" +
                                     $"L'année de fin sera ajustée à {anneeActuelle + 20}.");
                modelePrincipal.AnneeFin = (anneeActuelle + 20).ToString();
            }
            // Si valide → on garde ce que l utilisateur a entre dans le footer
        }

        // Navigation vers la page résultat
        IDataService dataService = Program.ServiceProvider.GetRequiredService<IDataService>();
        modelePrincipal.PageCourante = new ResultatViewModel(modelePrincipal, dataService);
    }

    /// <summary>
    /// Affiche une fenetre d erreur avec un message
    /// </summary>
    private async Task AfficherErreur(string message)
    {
        Window? fenetreParente = TopLevel.GetTopLevel(this) as Window;
        if (fenetreParente == null) return;

        TextBlock messageErreur = new TextBlock
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap
        };

        Button boutonOk = new Button
        {
            Width = 125,
            HorizontalAlignment = HorizontalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            Content = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 6,
                Children =
                {
                    new TextBlock
                    {
                        Text = "👍",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        FontSize = 24,
                        Margin = new Avalonia.Thickness(-5, 0, 5, 5),
                        FontWeight = FontWeight.Bold
                    },
                    new TextBlock
                    {
                        Text = "OK",
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 14
                    }
                }
            }
        };

        StackPanel contenuFenetre = new StackPanel
        {
            Margin = new Avalonia.Thickness(20),
            Spacing = 15,
            Children = { messageErreur, boutonOk }
        };

        Window fenetreErreur = new Window
        {
            Title = "Avertissement",
            Width = 420,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = contenuFenetre
        };

        boutonOk.Click += (expediteur, arguments) => fenetreErreur.Close();
        await fenetreErreur.ShowDialog(fenetreParente);
    }

    /// <summary>
    /// Lit l annee minimale depuis l objet @DATE du JSON
    /// </summary>
    private int ObtenirAnneeMinDepuisJson(string cheminFichier)
    {
        int anneeActuelle = DateTime.Now.Year;
        try
        {
            if (!File.Exists(cheminFichier)) return anneeActuelle - 10;

            string contenu = File.ReadAllText(cheminFichier);
            using System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(contenu);

            System.Text.Json.JsonElement racine = doc.RootElement;
            System.Text.Json.JsonElement pages = racine.GetProperty("_pages");

            foreach (System.Text.Json.JsonElement page in pages.EnumerateArray())
            {
                foreach (System.Text.Json.JsonElement objet in page.GetProperty("_objects").EnumerateArray())
                {
                    if (objet.GetProperty("_name").GetString() == "@DATE")
                    {
                        System.Text.Json.JsonElement obsMin = objet.GetProperty("_obs_min");
                        // Format : [numero_serie, "01/01/2016"]
                        string dateStr = obsMin[1].GetString() ?? "";
                        if (DateTime.TryParse(dateStr, out DateTime date))
                            return date.Year;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ERREUR JSON annee min] {ex.Message}");
        }
        return anneeActuelle - 10;
    }

    /// <summary>
    /// Lit l annee maximale depuis l objet @DATE du JSON
    /// </summary>
    private int ObtenirAnneeMaxDepuisJson(string cheminFichier)
    {
        int anneeActuelle = DateTime.Now.Year;
        try
        {
            if (!File.Exists(cheminFichier)) return anneeActuelle + 20;

            string contenu = File.ReadAllText(cheminFichier);
            using System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(contenu);

            System.Text.Json.JsonElement racine = doc.RootElement;
            System.Text.Json.JsonElement pages = racine.GetProperty("_pages");

            foreach (System.Text.Json.JsonElement page in pages.EnumerateArray())
            {
                foreach (System.Text.Json.JsonElement objet in page.GetProperty("_objects").EnumerateArray())
                {
                    if (objet.GetProperty("_name").GetString() == "@DATE")
                    {
                        System.Text.Json.JsonElement obsMax = objet.GetProperty("_obs_max");
                        // Format : [numero_serie, "10/01/2046"]
                        string dateStr = obsMax[1].GetString() ?? "";
                        if (DateTime.TryParse(dateStr, out DateTime date))
                            return date.Year;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ERREUR JSON annee max] {ex.Message}");
        }
        return anneeActuelle + 20;
    }

  
    private void btn_RetourAccueil_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindowViewModel modelePrincipal = (MainWindowViewModel)this.VisualRoot?.DataContext;

        if (modelePrincipal != null)
        {
            // 1. On applique le changement de page
            modelePrincipal.PageCourante = new AccueilViewModel(modelePrincipal);

            // 2. On nettoie le header et les états de validation
            modelePrincipal.Tableau = Environment.UserName;
            modelePrincipal.Banque1 = "";
            modelePrincipal.Banque2 = "";

            //  On réinitialise les années pour le prochain fichier
            modelePrincipal.AnneeDebut = "";
            modelePrincipal.AnneeFin = "";
        }
    }
}