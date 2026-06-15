using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AvaloniaApplication1;
using EF_Client_App_BL;
using EF_Client_App_DAL;
using EF_Client_App_Entity;

namespace EF_Client_UI_Avalonia
{
    public interface IDataService
    {
        List<EF_Client_App_Entity.Array> Arrays { get; }
        List<Description> Descriptions { get; }
        Task LoadAllDataAsync();
    }

    public class DataService : IDataService
    {
        public List<EF_Client_App_Entity.Array> Arrays { get; } = new();
        public List<Description> Descriptions { get; } = new();

        public async Task LoadAllDataAsync()
        {
            Arrays.Clear();
            Descriptions.Clear();

            using var httpClient = new HttpClient();
            var apiClient = new MetaDataClient(httpClient) { BaseUrl = App.ApiBaseUrl };

            // Charger le Canada (Mode/Modèle 1)
            var canadaArrays = await apiClient.GetArraysAsync(1);
            foreach (var array in canadaArrays) Arrays.Add(array.ToEntity());

            // DE BLOCAGE QUÉBEC : On tente de charger le Québec (Modèle 2 ou autre selon votre API)
            // Si votre API prend un autre paramètre pour le Québec, ajustez le chiffre ici
            try
            {
                var quebecArrays = await apiClient.GetArraysAsync(2);
                foreach (var array in quebecArrays) Arrays.Add(array.ToEntity());
            }
            catch { /* Sécurité au cas où l'ID 2 n'est pas le bon */ }

            // Charger les descriptions (ajustez également si un GetDescriptionsAsync(2) est requis)
            var canadaDescr = await apiClient.GetDescriptionsAsync(1);
            foreach (var descr in canadaDescr) Descriptions.Add(descr.ToEntity());

            try
            {
                var quebecDescr = await apiClient.GetDescriptionsAsync(2);
                foreach (var descr in quebecDescr) Descriptions.Add(descr.ToEntity());
            }
            catch { }

            // Reste du code pour le fichier JSON (Matching)...
            string cheminDossier = App.CheminJson;
            var fichiersJson = Directory.GetFiles(cheminDossier, "*.json");
            if (fichiersJson.Length > 0)
            {
                string filePath = fichiersJson[0];
                ReaderDepot readerDepot = DataFactory.CreateReader(filePath);
                var loadedSeries = readerDepot.Read(filePath);

                Matching matches = new Matching(Arrays, Descriptions, loadedSeries);
                matches.PopulateDescriptionsSeries();
            }
        }
    }
}