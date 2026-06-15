using EF_Client_App_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace EF_Client_App_DAL
{
    public class ReaderDepot
    {
        private IReader m_strategy;

        public ReaderDepot(IReader strategy)
        {
            m_strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public void SetStrategy(IReader strategy)
        {
            m_strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public List<Serie> Read(string source) => m_strategy.Read(source);
    }

}
