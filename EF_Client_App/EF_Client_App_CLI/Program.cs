using EF_Client_App_BL;
using EF_Client_App_DAL;
using EF_Client_App_Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    static async Task Main(string[] args)
    {
        // ==============================
        // CONFIGURATION APPSETTINGS
        // ==============================

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var baseUrl = configuration["ApiSettings:BaseUrl"];

        using var httpClient = new HttpClient();

        var apiClient = new MetaDataClient(httpClient)
        {
            BaseUrl = baseUrl
        };

 

        List<EF_Client_App_Entity.Array> arrays = new();
        List<EF_Client_App_Entity.Description> descriptions = new();


        try
        {
            // =====================================================
            // ARRAYS
            // =====================================================

            var arrayDtos = await apiClient.GetArraysAsync(1);
            var arrayDtos2 = await apiClient.GetArraysAsync(2);
            var arrayDtos3 = await apiClient.GetArraysAsync(3);
            var arrayDtos4 = await apiClient.GetArraysAsync(4);


            foreach(ArrayDTO array in arrayDtos)
            {
                arrays.Add(array.ToEntity());
                Console.WriteLine(array.Title);

            }

            foreach (ArrayDTO array in arrayDtos2)
            {
                arrays.Add(array.ToEntity());
                Console.WriteLine(array.Title);

            }

            foreach (ArrayDTO array in arrayDtos3)
            {
                arrays.Add(array.ToEntity());
                Console.WriteLine(array.Title);

            }

            foreach (ArrayDTO array in arrayDtos4)
            {
                arrays.Add(array.ToEntity());
                Console.WriteLine(array.Title);

            }

            Console.WriteLine("=== Arrays ===");
            Console.WriteLine($"{arrays.Count} array(s) chargé(s)");

            // =====================================================
            // DESCRIPTIONS
            // =====================================================

            var descriptionDtos = await apiClient.GetDescriptionsAsync(1);

            foreach( DescriptionDTO descr in descriptionDtos)
            {
                descriptions.Add(descr.ToEntity());

            }

           

            Console.WriteLine("\n=== Descriptions ===");
            Console.WriteLine($"{descriptions.Count} description(s) chargée(s)");

            // =====================================================
            // NOTES
            // =====================================================

            var notes = await apiClient.GetNotesAsync(1);

            Console.WriteLine("\n=== Notes ===");
            Console.WriteLine($"{notes.Count} note(s) chargée(s)");


            // =====================================================
            // Histo
            // =====================================================

            var histoDtos = await apiClient.GetHistoricAsync("aaa","prevcan");

            var historic = histoDtos.ToEntity();

            Console.WriteLine("=== historic ===");
            Console.WriteLine($"{historic.UserId} {historic.PRN_Selection}{historic.StartedQuarter}");

            var histosave = new HistoricDTO("bbb","prevcan",null,null,1,1,1,1);

            await apiClient.SaveHistoricAsync(histosave);

            var histosaves = await apiClient.GetHistoricAsync("bbb", "prevcan");

            var historicS = histosaves.ToEntity();

            Console.WriteLine($"{historicS.UserId} {historicS.PRN_Selection}{historicS.StartedQuarter}");


            var histoup = new HistoricDTO("bbb", "prevcan", null, null, 3, 3, 3, 3);

            await apiClient.UpdateHistoricAsync(histoup);

            var histoups = await apiClient.GetHistoricAsync("bbb", "prevcan");

            var historicupd = histoups.ToEntity();


            Console.WriteLine($"{historicupd.UserId} {historicupd.PRN_Selection}{historicupd.StartedQuarter}");




        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur générale :");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            return;
        }

        // =====================================================
        // READER
        // =====================================================

        string filePath =
            @"C:\PROJET SYNTHESE\TableauxPrev2026\TableauxPrev2026\Banques\json\compcanwf1.json";

        // Vérification du fichier
        if (!File.Exists(filePath))
        {
            Console.WriteLine("✘ Fichier introuvable : " + filePath);
            return;
        }

        try
        {
            ReaderDepot readerDepot = DataFactory.CreateReader(filePath);

            List<Serie> series = readerDepot.Read(filePath);

            Console.WriteLine(
                $"\n── {series.Count} série(s) chargée(s) depuis : {filePath}");

            // =====================================================
            // MATCHING
            // =====================================================

            Matching matches = new Matching(arrays, descriptions, series);

   
            matches.PopulateDescriptionsSeries();

            DateOnly date = DateOnly.Parse("2021-07-01");

            matches.RenderSeriesValuesByYear(101,'Q', date);

           // matches.TestRender(101);

            /*
            foreach (Serie serie in series)
            {
                Console.WriteLine();
                Console.WriteLine($"  ┌─ {serie.ID}");
                Console.WriteLine($"  │  Mnémonique      : {serie.Mnemonic}");
                Console.WriteLine($"  │  Description     : {serie.Description}");
                Console.WriteLine($"  │  Source          : {serie.Source}");
                Console.WriteLine($"  │  Dernière MAJ    : {serie.LastUpdate}");
                Console.WriteLine($"  │  Première période: {serie.FirstPeriod}");
                Console.WriteLine($"  │  Dernière période: {serie.LastPeriod}");
                Console.WriteLine($"  │  Fréquence       : {serie.Frequency}");
                Console.WriteLine($"  │  Banque          : {serie.Bank}");
                Console.WriteLine($"  │  Unité           : {serie.Unity}");
                Console.WriteLine($"  │  Nb observations : {serie.Observations.Count}");

                int n = 0;

                foreach (KeyValuePair<string, decimal> kvp in serie.Observations)
                {
                    Console.WriteLine($"  │    {kvp.Key}  :  {kvp.Value}");

                    if (++n >= 10)
                    {
                        Console.WriteLine("  │    ...");
                        break;
                    }
                }

                Console.WriteLine("  └────────────────────────────────────────");
            }
            */
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine($"✘ Format non supporté : {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✘ Erreur lors de la lecture : {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }

        Console.WriteLine("\nAppuyez sur une touche pour quitter...");
        Console.ReadKey();
    }
}