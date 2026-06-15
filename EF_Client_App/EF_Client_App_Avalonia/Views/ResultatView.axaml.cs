using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using AvaloniaApplication1.ViewModels;
using EF_Client_App_Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace AvaloniaApplication1.Views;

public partial class ResultatView : UserControl
{
    public ResultatView()
    {
        InitializeComponent();
        this.DataContextChanged += ResultatView_DataContextChanged;
    }

    private void ResultatView_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is ResultatViewModel vm)
        {
            // Rebuild columns when descriptions are filtered/updated
            vm.Descriptions.CollectionChanged += (_, _) => BuildColumns(vm);
            BuildColumns(vm);
        }
    }

    private void BuildColumns(ResultatViewModel vm)
    {
        // Execute thread-safely on UI thread to prevent DataGrid concurrency exceptions
        Dispatcher.UIThread.Post(() =>
        {
            // Clear all columns to avoid index/RemoveAt issues
            ResultsDataGrid.Columns.Clear();

            // Re-add the fixed Description column
            ResultsDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Description",
                Binding = new Binding("FormattedDescription"),
                Width = new DataGridLength(350)
            });

            if (!vm.Descriptions.Any()) return;

            // --- Récupérer le ViewModel principal pour lire les filtres ---
            var mainVM = this.FindAncestorOfType<Window>()?.DataContext as MainWindowViewModel;
            var mode   = mainVM?.ModePeriode ?? ModePeriode.Trimestriel;
            bool estComparaison = mainVM?.TypeRecherche?.Contains("Comparaison", StringComparison.OrdinalIgnoreCase) == true;

            // --- Générer dynamiquement les clés de colonnes ---
            var allKeys = GenererClesPeriodes(mainVM);

            foreach (var key in allKeys)
            {
                var capturedKey  = key;
                var capturedMode = mode;

                var col = new DataGridTemplateColumn
                {
                    Header = DateToQuarter(key, capturedMode),
                    Width  = new DataGridLength(110),
                    CellTemplate = new FuncDataTemplate<Description>((desc, _) =>
                    {
                        var sp = new StackPanel { Margin = new Avalonia.Thickness(4, 4) };
                        var alreadyAdded = new HashSet<string>();

                        // Ligne 1 : FirstLineArray (valeur principale)
                        string format1 = GetFormat(desc, 1);
                        AddSeriesValues(sp, desc.FirstLineArray, capturedKey, format1, capturedMode, alreadyAdded, estComparaison);

                        // Ligne 2 : Line3LevelSpec (niveau / variation)
                        string format2 = GetFormat(desc, 2);
                        AddSeriesValues(sp, desc.Line3LevelSpec, capturedKey, format2, capturedMode, alreadyAdded, estComparaison);

                        // Ligne 3 : Line4PchCont (pch cont)
                        string format3 = GetFormat(desc, 3);
                        AddSeriesValues(sp, desc.Line4PchCont, capturedKey, format3, capturedMode, alreadyAdded, estComparaison);

                        return sp;
                    })
                };
                ResultsDataGrid.Columns.Add(col);
            }
        });
    }

    // -------------------------------------------------------
    // Génération dynamique des clés de colonnes
    // -------------------------------------------------------
    private static List<string> GenererClesPeriodes(MainWindowViewModel? mainVM)
    {
        // Lire les filtres avec valeurs de repli sécurisées
        int anneeDebut = int.TryParse(mainVM?.AnneeDebut, out int ad)
                         ? ad : DateTime.Now.Year - 2;
        int anneeFin   = int.TryParse(mainVM?.AnneeFin,   out int af)
                         ? af : DateTime.Now.Year + 2;

        // S'assurer que l'ordre est cohérent
        if (anneeDebut > anneeFin) (anneeDebut, anneeFin) = (anneeFin, anneeDebut);

        int moisDebut = TrimestreVersMois(mainVM?.TrimestreDebut ?? "I");
        int moisFin   = TrimestreVersMois(mainVM?.TrimestreFin   ?? "IV");

        var mode = mainVM?.ModePeriode ?? ModePeriode.Trimestriel;
        var keys = new List<string>();

        for (int annee = anneeDebut; annee <= anneeFin; annee++)
        {
            if (mode == ModePeriode.Annuel)
            {
                // Une seule colonne par an (séries A)
                keys.Add($"{annee}-01-01");
            }
            else
            {
                // Mode Trimestriel ou TrimestrielAnnuel : colonnes Q
                foreach (int mois in new[] { 1, 4, 7, 10 })
                {
                    // Filtre trimestre début sur la première année
                    if (annee == anneeDebut && mois < moisDebut) continue;
                    // Filtre trimestre fin sur la dernière année
                    if (annee == anneeFin   && mois > moisFin)   continue;

                    keys.Add($"{annee}-{mois:D2}-01");
                }
            }
        }

        return keys;
    }

    private static int TrimestreVersMois(string trimestre) => trimestre switch
    {
        "I"   => 1,
        "II"  => 4,
        "III" => 7,
        "IV"  => 10,
        _     => 1
    };

    // -------------------------------------------------------
    // Helpers d'affichage
    // -------------------------------------------------------
    private static string GetFormat(Description desc, int lineType)
    {
        int decimals = desc.Decimal ?? (lineType == 1 ? 0 : 1);
        return "N" + decimals;
    }

    /// <summary>
    /// Ajoute les valeurs d'une liste de séries dans le StackPanel de la cellule.
    /// Filtre selon la fréquence attendue par le mode :
    ///   Trimestriel       → 'Q' seulement
    ///   Annuel            → 'A' seulement
    ///   TrimestrielAnnuel → 'Q' ET 'A'
    /// </summary>
    private static void AddSeriesValues(
        StackPanel sp, List<Serie>? series, string key, string format, ModePeriode mode, HashSet<string> alreadyAdded, bool estComparaison)
    {
        if (series == null) return;

        foreach (var serie in series)
        {
            if (serie == null) continue;

            // Filtre par fréquence selon le mode actif
            bool inclure = mode switch
            {
                ModePeriode.Trimestriel       => serie.Frequency == 'Q',
                ModePeriode.Annuel            => serie.Frequency == 'A',
                ModePeriode.TrimestrielAnnuel => true,   // Q et A affichés
                _                             => true
            };
            if (!inclure) continue;

            string text = "";
            if (serie.Observations?.TryGetValue(key, out decimal val) == true)
                text = val.ToString(format, CultureInfo.GetCultureInfo("fr-FR"));

            if (!estComparaison)
            {
                if (alreadyAdded.Contains(text)) continue;
                alreadyAdded.Add(text);
            }

            sp.Children.Add(new TextBlock
            {
                Text          = text,
                TextAlignment = TextAlignment.Right,
                FontSize      = 11,
                Margin        = new Avalonia.Thickness(0, 1)
            });
        }
    }

    /// <summary>
    /// Convertit une clé "yyyy-MM-dd" en entête lisible.
    /// Mode Annuel : "2024"
    /// Autres      : "2024-I", "2024-II", …
    /// </summary>
    private static string DateToQuarter(string dateKey, ModePeriode mode)
    {
        if (DateTime.TryParse(dateKey, out DateTime dt))
        {
            if (mode == ModePeriode.Annuel)
                return dt.Year.ToString();

            string q = dt.Month switch
            {
                1  => "I",
                4  => "II",
                7  => "III",
                10 => "IV",
                _  => dt.Month.ToString()
            };
            return $"{dt.Year}-{q}";
        }
        return dateKey;
    }
}