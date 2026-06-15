using EF_Client_App_DAL.CSV;
using EF_Client_App_DAL.EVIEWS;
using EF_Client_App_DAL.JSON;
using EF_Client_App_Entity;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace EF_Client_App_DAL
{
    public static class DataFactory
    {
        public static ReaderDepot CreateReader(string filePath)
        {
            string ext = Path.GetExtension(filePath)?.ToLowerInvariant() ?? string.Empty;

            IReader strategy = ext switch
            {
                ".json" => new JSONReader(),
                ".csv" => new CSVReader(),
                ".wf1" or ".wf" => new EViewsReader(),
                _ => throw new NotSupportedException(
                         $"Extension '{ext}' non supportée. " +
                         $"Extensions acceptées : .json, .csv, .wf1, .wf")
            };

            return new ReaderDepot(strategy);
        }
    }
}
