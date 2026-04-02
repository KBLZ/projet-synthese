using PE_DAL.Oracle.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_HistoriqueRepository
    {

        private readonly PE_DBContext m_context;

        public DTO_HistoriqueRepository(PE_DBContext context)
        {
            m_context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<T> LireDonnees<T>(string? parametre = null)
        {
            if (typeof(T) == typeof(DTO_Historique))
            {
                IEnumerable<DTO_Historique> resultat = m_context.Historiques.AsEnumerable();

                if (!string.IsNullOrEmpty(parametre))
                {
                    resultat = resultat.Where(h => h.ChoixPRN == parametre || h.IdUtilisateur == parametre);
                }

                return (IEnumerable<T>)resultat;
            }

            throw new NotSupportedException();
        }

        public void Dispose()
        {
            m_context.Dispose();
        }
    }
}
