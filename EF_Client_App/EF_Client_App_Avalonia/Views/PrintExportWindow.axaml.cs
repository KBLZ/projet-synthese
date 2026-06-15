using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaApplication1.ViewModels;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Views
{
    public partial class PrintExportWindow : Window
    {
        public PrintExportWindow()
        {
            InitializeComponent();
        }

        public PrintExportWindow(PrintExportViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            // On branche le dialogue de sauvegarde ici (nécessite la référence à la fenêtre)
            vm.DemanderCheminSauvegarde = async () =>
            {
                var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Exporter vers Excel",
                    DefaultExtension = "xlsx",
                    FileTypeChoices = new[]
                    {
                        new FilePickerFileType("Fichier Excel") { Patterns = new[] { "*.xlsx" } }
                    }
                });
                return file?.Path.LocalPath;
            };

            vm.FermerFenetre = () => Close();
        }
    }
}