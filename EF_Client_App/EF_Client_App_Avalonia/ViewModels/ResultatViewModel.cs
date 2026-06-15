using CommunityToolkit.Mvvm.ComponentModel;
using EF_Client_App_BL;
using EF_Client_App_DAL;
using EF_Client_App_Entity;
using EF_Client_UI_Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class ResultatViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainVM;
        private readonly IDataService _dataService;
        private readonly List<Description> _allDescriptions = new();

        // Utilisation d'une propriété standard pour l'UI
        public ObservableCollection<Description> Descriptions { get; } = new();

        // Cette propriété lit directement le titre de façon dynamique
        public string SelectedArrayTitle => _mainVM.SelectedArray?.Title ?? "Aucun tableau sélectionné";

        public ResultatViewModel(MainWindowViewModel mainVM, IDataService dataService)
        {
            _mainVM = mainVM;
            _dataService = dataService;

            _mainVM.IsAccueil = false;

            // Les données sont déjà chargées globalement, on remplit juste les listes locales
            _allDescriptions.Clear();
            _allDescriptions.AddRange(_dataService.Descriptions);

            // Rafraîchir la sidebar et sélectionner le premier tableau disponible
            _mainVM.RafraichirSidebar();

            var defaultArray = _mainVM.Arrays.FirstOrDefault();
            if (defaultArray != null)
            {
                _mainVM.SelectedArray = defaultArray;
            }

            ExecuterFiltrage();
        }
        /*
        public ResultatViewModel(MainWindowViewModel mainVM, IDataService dataService)
        {
            _mainVM = mainVM;
            _dataService = dataService;

            _mainVM.IsAccueil = false;
            _mainVM.Arrays.Clear();

            // Déclenchement du chargement en arrière-plan de manière totalement isolée
            Task.Run(async () => await InitialiserDonneesAsync());
        }

        private async Task InitialiserDonneesAsync()
        {
            try
            {
                // 1. Chargement asynchrone pur (hors thread UI)
                if (_dataService.Arrays.Count == 0)
                {
                    await _dataService.LoadAllDataAsync().ConfigureAwait(false);
                }

                // Cache local des descriptions
                _allDescriptions.Clear();
                _allDescriptions.AddRange(_dataService.Descriptions);

                // 2. Retour unique et synchronisé sur le thread UI pour l'affichage graphique
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    // Remplissage séquentiel de la Sidebar
                    _mainVM.RafraichirSidebar();

                    // Sélection sécurisée du premier tableau disponible
                    var defaultArray = _mainVM.Arrays.FirstOrDefault();
                    if (defaultArray != null)
                    {
                        _mainVM.SelectedArray = defaultArray;
                    }

                    // Forcer le premier filtrage visuel des descriptions
                    ExecuterFiltrage();
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERREUR] Échec de l'initialisation : {ex.Message}");
            }
        }
        */


        /// <summary>
        /// Cette méthode est appelée directement par le XAML ou par votre Sidebar 
        /// lors d'un clic sur un bouton (via le changement de SelectedArray)
        /// </summary>
        public void RafraichirSelectionApresClic()
        {
            OnPropertyChanged(nameof(SelectedArrayTitle));
            ExecuterFiltrage();
        }

        private void ExecuterFiltrage()
        {
            Descriptions.Clear();

            if (_mainVM.SelectedArray == null)
            {
                foreach (var d in _allDescriptions)
                {
                    Descriptions.Add(d);
                }
            }
            else
            {
                var descriptionsFiltrees = _allDescriptions.Where(d => d.ArrayId == _mainVM.SelectedArray.ArrayId);
                foreach (var d in descriptionsFiltrees)
                {
                    Descriptions.Add(d);
                }
            }
        }
    }
}