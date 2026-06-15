using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EF_Client_App_Entity;
using EF_Client_UI_Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class PrintExportViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly bool _estComparaison;
        private readonly string _banque1;
        private readonly string _banque2;
        private readonly string _typeRecherche;

        public ObservableCollection<TableauSelectionnableVM> TableauxDisponibles { get; } = new();

        // Callbacks branchés depuis la View
        public Func<Task<string?>>? DemanderCheminSauvegarde { get; set; }
        public Action? FermerFenetre { get; set; }

        [ObservableProperty]
        private bool _selectAll;

        [ObservableProperty]
        private bool _aucuneSelection;

        // Trimestres affichés — identiques à ResultatView
        private static readonly List<string> Trimestres = new()
        {
            "2023-10-01", "2024-01-01", "2024-04-01", "2024-07-01", "2024-10-01",
            "2025-01-01", "2025-04-01", "2025-07-01", "2025-10-01"
        };

        // ── Constructeur ─────────────────────────────────────────────────────
        public PrintExportViewModel(
            IEnumerable<BoutonTableauDummy> tableauxSidebar,
            IDataService dataService,
            string typeRecherche,
            string banque1,
            string banque2)
        {
            _dataService = dataService;
            _typeRecherche = typeRecherche;
            _banque1 = banque1;
            _banque2 = banque2;
            _estComparaison = typeRecherche.Contains("Comparaison", StringComparison.OrdinalIgnoreCase);

            foreach (var b in tableauxSidebar)
            {
                var item = new TableauSelectionnableVM
                {
                    Numero = b.Numero,
                    Titre = b.Titre,
                    OriginalArrayId = b.OriginalArrayId,
                    IsSelected = false
                };
                item.PropertyChanged += (_, _) => SynchroniserEtat();
                TableauxDisponibles.Add(item);
            }
        }

        // ── SelectAll ────────────────────────────────────────────────────────
        partial void OnSelectAllChanged(bool value)
        {
            foreach (var t in TableauxDisponibles)
                t.IsSelected = value;
            AucuneSelection = false;
        }

        private void SynchroniserEtat()
        {
            var tousCoches = TableauxDisponibles.All(t => t.IsSelected);
            if (_selectAll != tousCoches)
            {
                _selectAll = tousCoches;
                OnPropertyChanged(nameof(SelectAll));
            }
            AucuneSelection = false;
        }

        // ── Export ───────────────────────────────────────────────────────────
        [RelayCommand]
        private async Task Export()
        {
            var selectionnes = TableauxDisponibles.Where(t => t.IsSelected).ToList();
            if (!selectionnes.Any()) { AucuneSelection = true; return; }
            if (DemanderCheminSauvegarde is null) return;

            var path = await DemanderCheminSauvegarde();
            if (path is null) return;

            BuildWorkbook(selectionnes, path);
        }

        // ── Impression ───────────────────────────────────────────────────────
        [RelayCommand]
        private void Print()
        {
            var selectionnes = TableauxDisponibles.Where(t => t.IsSelected).ToList();
            if (!selectionnes.Any()) { AucuneSelection = true; return; }

            var tempPath = Path.Combine(Path.GetTempPath(), $"impression_{Guid.NewGuid()}.xlsx");
            BuildWorkbook(selectionnes, tempPath);

            Process.Start(new ProcessStartInfo(tempPath)
            {
                Verb = "print",
                UseShellExecute = true
            });
        }

        // ── Fermer ───────────────────────────────────────────────────────────
        [RelayCommand]
        public void Close() => FermerFenetre?.Invoke();

        // ── Construction du classeur Excel ───────────────────────────────────
        private void BuildWorkbook(List<TableauSelectionnableVM> tableaux, string path)
        {
            using var workbook = new XLWorkbook();

            foreach (var tableau in tableaux)
            {
                string nomOnglet = $"{tableau.Numero} {tableau.Titre}";
                if (nomOnglet.Length > 31) nomOnglet = nomOnglet[..31];

                var sheet = workbook.Worksheets.Add(nomOnglet);

                if (_estComparaison)
                    RemplirFeuilleComparaison(sheet, tableau);
                else
                    RemplirFeuillePrevsion(sheet, tableau);

                sheet.Column(1).Width = 48;
                sheet.Columns(2, sheet.LastColumnUsed()?.ColumnNumber() ?? 2).Width = 14;
            }

            workbook.SaveAs(path);
        }

        // ── MODE PRÉVISION ───────────────────────────────────────────────────
        // Structure : 1 ligne Description | colonnes = trimestres
        // Pour chaque description : 3 sous-lignes (FirstLineArray, Line3LevelSpec, Line4PchCont)
        private void RemplirFeuillePrevsion(IXLWorksheet sheet, TableauSelectionnableVM tableau)
        {
            // ── Titre du tableau ──
            sheet.Cell(1, 1).Value = _typeRecherche;
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 13;

            sheet.Cell(2, 1).Value = $"{tableau.Numero} — {tableau.Titre}";
            sheet.Cell(2, 1).Style.Font.Italic = true;

            // ── En-têtes de colonnes (ligne 4) ──
            sheet.Cell(4, 1).Value = "Description";
            for (int i = 0; i < Trimestres.Count; i++)
                sheet.Cell(4, i + 2).Value = DateToQuarter(Trimestres[i]);

            StyleEnTete(sheet.Range(4, 1, 4, Trimestres.Count + 1));

            // ── Données ──
            var descriptions = _dataService.Descriptions
                .Where(d => d.ArrayId == tableau.OriginalArrayId)
                .ToList();

            int row = 5;
            foreach (var desc in descriptions)
            {
                string format1 = "N" + (desc.Decimal ?? 0);
                string format2 = "N1";
                string format3 = "N1";

                bool aLigne2 = desc.Line3LevelSpec?.Any() == true;
                bool aLigne3 = desc.Line4PchCont?.Any() == true;
                int nbSousLignes = 1 + (aLigne2 ? 1 : 0) + (aLigne3 ? 1 : 0);

                // Colonne Description — fusionnée sur les sous-lignes si nécessaire
                if (nbSousLignes > 1)
                {
                    sheet.Range(row, 1, row + nbSousLignes - 1, 1).Merge();
                    sheet.Cell(row, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }
                sheet.Cell(row, 1).Value = desc.TextDescription ?? "";
                sheet.Cell(row, 1).Style.Alignment.WrapText = true;

                // Sous-ligne 1 : FirstLineArray
                EcrireValeursSerie(sheet, row, desc.FirstLineArray, format1);

                int sousRow = row + 1;

                // Sous-ligne 2 : Line3LevelSpec
                if (aLigne2)
                {
                    EcrireValeursSerie(sheet, sousRow, desc.Line3LevelSpec, format2,
                        couleurFond: XLColor.FromHtml("#EEF4FB"));
                    sousRow++;
                }

                // Sous-ligne 3 : Line4PchCont
                if (aLigne3)
                {
                    EcrireValeursSerie(sheet, sousRow, desc.Line4PchCont, format3,
                        couleurFond: XLColor.FromHtml("#F5F5F5"));
                }

                // Séparateur visuel entre descriptions
                sheet.Range(row + nbSousLignes - 1, 1, row + nbSousLignes - 1,
                    Trimestres.Count + 1).Style.Border.BottomBorder = XLBorderStyleValues.Medium;

                row += nbSousLignes;
            }
        }

        // ── MODE COMPARAISON ─────────────────────────────────────────────────
        // Structure : pour chaque trimestre, 2 colonnes (Banque1 | Banque2)
        // ArrayId simple (101-222) = Banque1, ArrayId comparaison (301-422) = Banque2
        private void RemplirFeuilleComparaison(IXLWorksheet sheet, TableauSelectionnableVM tableau)
        {
            // L'ArrayId affiché dans la sidebar est soit la version simple soit comparaison
            // On retrouve les deux IDs correspondants
            int idSimple, idComparaison;
            DeterminerIdsComparaison(tableau.OriginalArrayId, out idSimple, out idComparaison);

            var descSimple = _dataService.Descriptions
                .Where(d => d.ArrayId == idSimple).ToList();
            var descComparaison = _dataService.Descriptions
                .Where(d => d.ArrayId == idComparaison).ToList();

            // ── Titre ──
            sheet.Cell(1, 1).Value = _typeRecherche;
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 13;

            sheet.Cell(2, 1).Value = $"{tableau.Numero} — {tableau.Titre}";
            sheet.Cell(2, 1).Style.Font.Italic = true;

            // ── En-têtes : col 1 = Description, puis par trimestre 2 colonnes (B1 | B2) ──
            int nbTrimestres = Trimestres.Count;
            int totalCols = 1 + nbTrimestres * 2;

            sheet.Cell(4, 1).Value = "Description";

            for (int i = 0; i < nbTrimestres; i++)
            {
                int colBase = 2 + i * 2;
                string labelTrimestre = DateToQuarter(Trimestres[i]);

                // Fusionner les 2 colonnes pour le label du trimestre
                sheet.Range(4, colBase, 4, colBase + 1).Merge();
                sheet.Cell(4, colBase).Value = labelTrimestre;
                sheet.Cell(4, colBase).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Ligne 5 : sous-en-têtes Banque1 / Banque2
                sheet.Cell(5, colBase).Value = _banque1;
                sheet.Cell(5, colBase + 1).Value = _banque2;
            }

            StyleEnTete(sheet.Range(4, 1, 5, totalCols));

            // ── Données (ligne 6+) ──
            int row = 6;
            int nbDesc = Math.Max(descSimple.Count, descComparaison.Count);

            for (int d = 0; d < nbDesc; d++)
            {
                var ds = d < descSimple.Count ? descSimple[d] : null;
                var dc = d < descComparaison.Count ? descComparaison[d] : null;

                string format = "N" + ((ds ?? dc)?.Decimal is int dec ? dec : 0);

                sheet.Cell(row, 1).Value = ds?.TextDescription ?? dc?.TextDescription ?? "";
                sheet.Cell(row, 1).Style.Alignment.WrapText = true;

                for (int t = 0; t < nbTrimestres; t++)
                {
                    string key = Trimestres[t];
                    int colBase = 2 + t * 2;

                    sheet.Cell(row, colBase).Value = ExtraireValeur(ds?.FirstLineArray, key, format);
                    sheet.Cell(row, colBase + 1).Value = ExtraireValeur(dc?.FirstLineArray, key, format);
                }

                // Alternance de couleur pour lisibilité
                if (row % 2 == 0)
                    sheet.Range(row, 1, row, totalCols).Style.Fill
                         .BackgroundColor = XLColor.FromHtml("#F9F9F9");

                row++;
            }
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private void EcrireValeursSerie(IXLWorksheet sheet, int row,
            List<Serie>? series, string format, XLColor? couleurFond = null)
        {
            for (int t = 0; t < Trimestres.Count; t++)
            {
                string key = Trimestres[t];
                string val = ExtraireValeur(series, key, format);
                var cell = sheet.Cell(row, t + 2);
                cell.Value = val;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                if (couleurFond != null)
                    cell.Style.Fill.BackgroundColor = couleurFond;
            }
        }

        private static void DeterminerIdsComparaison(int arrayId, out int idSimple, out int idComparaison)
        {
            // Québec : simple 201-222, comparaison 401-422 (écart de 200)
            // Canada : simple 101-130, comparaison 301-330 (écart de 200)
            if (arrayId >= 401) { idComparaison = arrayId; idSimple = arrayId - 200; }
            else if (arrayId >= 301) { idComparaison = arrayId; idSimple = arrayId - 200; }
            else if (arrayId >= 201) { idSimple = arrayId; idComparaison = arrayId + 200; }
            else { idSimple = arrayId; idComparaison = arrayId + 200; }
        }

        private static void StyleEnTete(IXLRange range)
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.BackgroundColor = XLColor.FromHtml("#2E75B6");
            range.Style.Font.FontColor = XLColor.White;
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        private static string ExtraireValeur(List<Serie>? series, string key, string format)
        {
            if (series == null) return "";
            foreach (var serie in series)
                if (serie?.Observations?.TryGetValue(key, out decimal val) == true)
                    return val.ToString(format, CultureInfo.GetCultureInfo("fr-FR"));
            return "";
        }

        private static string DateToQuarter(string dateKey)
        {
            if (DateTime.TryParse(dateKey, out DateTime dt))
            {
                string q = dt.Month switch { 1 => "I", 4 => "II", 7 => "III", 10 => "IV", _ => "" };
                return $"{dt.Year}-{q}";
            }
            return dateKey;
        }
    }

    public partial class TableauSelectionnableVM : ObservableObject
    {
        public string Numero { get; set; } = "";
        public string Titre { get; set; } = "";
        public int OriginalArrayId { get; set; }

        [ObservableProperty]
        private bool _isSelected;
    }
}