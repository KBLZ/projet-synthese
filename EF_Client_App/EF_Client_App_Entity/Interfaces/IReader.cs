using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Client_App_Entity
{
    public interface IReader
    {
        /// <summary>Lit la source et retourne une ou plusieurs Series normalisées.</summary>
        List<Serie> Read(string source);
    }
}
