using EF_Client_App_Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EF_Client_App_DAL.JSON
{
    public class JSONReader : IReader
    {
        public List<Serie> Read(string source)
        {
            string json = File.ReadAllText(source);
            JObject root = JObject.Parse(json);
            string wfName = root.Value<string>("_name") ?? source;

            var series = new List<Serie>();

            foreach (JObject page in root["_pages"]?.OfType<JObject>() ?? Enumerable.Empty<JObject>())
            {


                string freqStr = (page.Value<string>("_frequency") ?? "").Trim();
                string frequency = string.IsNullOrEmpty(freqStr) ? "UNKNOWN" : freqStr;
                char freq = frequency.Length > 0 ? frequency[0] : ' ';

                string pageMAJ = ExtraireDate(page["_last_update"]);

                // ── 1. Construire l'index des dates depuis @DATE ──────────────
                var indexDates = new List<string>();

                JObject? indexObj = page["_objects"]?
                    .OfType<JObject>()
                    .FirstOrDefault(o => o.Value<string>("_type") == "INDEX");

                if (indexObj != null)
                {
                    foreach (JToken entry in indexObj["_values"] ?? new JArray())
                    {
                        string dateStr = "";

                        if (entry is JArray arr && arr.Count >= 2)
                            dateStr = arr[1].Value<string>() ?? "";
                        else if (entry.Type != JTokenType.Null)
                            dateStr = entry.Value<string>() ?? "";

                        string isoDate = ConvertirDateISO(dateStr);
                        if (!string.IsNullOrEmpty(isoDate))
                            indexDates.Add(isoDate);
                    }
                }

                if (indexDates.Count == 0) continue;

                // ── 2. Lire chaque SERIES ─────────────────────────────────────
                foreach (JObject obj in page["_objects"]?.OfType<JObject>()
                                        ?? Enumerable.Empty<JObject>())
                {
                    if (obj.Value<string>("_type") != "SERIES") continue;

                    string serieName = obj.Value<string>("_name") ?? "INCONNU";
                    string serieMAJ = ExtraireDate(obj["_last_update"]);

                    var serieDto = new SerieDTO
                    {
                        ID = serieName,
                        Mnemonic = serieName,
                        Source = wfName,
                        Frequency = freq,
                        LastUpdate = string.IsNullOrEmpty(serieMAJ) ? pageMAJ : serieMAJ,
                        Bank = Path.GetFileNameWithoutExtension(wfName)
                    };
                   
                    JArray? values = obj["_values"] as JArray;

                    if (values == null || values.Count == 0)
                    {
                        series.Add(serieDto.ToEntity());
                        continue;
                    }

                    int count = Math.Min(values.Count, indexDates.Count);

                    for (int i = 0; i < count; i++)
                    {
                        JToken token = values[i];

                        // null = NA → on saute mais i reste aligné sur indexDates[i]
                        if (token.Type == JTokenType.Null) continue;

                        // ✅ Extraire directement la valeur numérique via Newtonsoft
                        // sans passer par ToString() qui peut produire "1E-05" etc.
                        decimal val;
                        try
                        {
                            val = token.Value<decimal>();
                        }
                        catch
                        {
                            // Dernier recours : parse manuel avec InvariantCulture
                            string raw = token.ToString();
                            if (!decimal.TryParse(raw,
                                    NumberStyles.Float | NumberStyles.AllowLeadingSign,
                                    CultureInfo.InvariantCulture,
                                    out val))
                            {
                                continue; // vraiment impossible → on ignore
                            }
                        }

                        serieDto.AddObservation(indexDates[i], val);
                    }

                    series.Add(serieDto.ToEntity());
                }
            }

           
            
            return series;
        }

        // [serial, "MM/DD/YYYY HH:mm"] → "MM/DD/YYYY HH:mm"
        private string ExtraireDate(JToken? token)
        {
            if (token is JArray arr && arr.Count >= 2)
                return arr[1].Value<string>() ?? "";
            return "";
        }

        // "MM/DD/YYYY" → "YYYY-MM-DD"
        private string ConvertirDateISO(string dateMDY)
        {
            if (string.IsNullOrWhiteSpace(dateMDY)) return "";

            if (DateTime.TryParseExact(
                    dateMDY, "MM/dd/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dt))
                return dt.ToString("yyyy-MM-dd");

            return dateMDY;
        }
    }
}