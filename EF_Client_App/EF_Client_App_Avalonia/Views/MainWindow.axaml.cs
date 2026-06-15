using Avalonia.Controls;
using AvaloniaApplication1.ViewModels;
using System;
using System.Linq;

namespace AvaloniaApplication1.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_RetourAccueil_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var mainVM = (MainWindowViewModel)this.DataContext;
            mainVM.AnnulerFiltres();
            mainVM.PageCourante = new AccueilViewModel(mainVM);

            // ====================================================================
            // Nettoyage du Header lors du retour depuis les résultats
            // ====================================================================
            mainVM.Tableau = Environment.UserName;
            mainVM.Banque1 = "";
            mainVM.Banque2 = "";
            mainVM.AnneeDebut = "";
            mainVM.AnneeFin = "";
        }

        private void btn_AnnulerFiltres_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var mainVM = (MainWindowViewModel)this.DataContext;
            mainVM.AnnulerFiltres();
        }

        private async void btn_ImpressionExport_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var mainVM = (MainWindowViewModel)this.DataContext!;

            if (!mainVM.ListeDeTest.Any()) return;

            var vm = new PrintExportViewModel(
                mainVM.ListeDeTest,
                mainVM.DataService,        // propriété publique à ajouter (voir étape 3)
                mainVM.TypeRecherche,      // "Prévisions du Québec" ou "Comparaison..."
                mainVM.Banque1,
                mainVM.Banque2
            );

            var window = new PrintExportWindow(vm);
            await window.ShowDialog(this);
        }

        private void todo(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Ne fait rien et c'est voulu.  C'est pour enlever des erreurs en attendant de définir tous les boutons
        }
    }
}