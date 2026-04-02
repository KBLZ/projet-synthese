using PE_DAL.Oracle.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_TableauRepository
    {

        private readonly PE_DBContext m_context;

        public DTO_TableauRepository(PE_DBContext context)
        {
            m_context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<T> LireDonnees<T>(string? parametre = null)
        {
            if (typeof(T) == typeof(DTO_Tableau))
            {
                IEnumerable<DTO_Tableau> resultat = m_context.Tableaux.AsEnumerable();

                if (!string.IsNullOrEmpty(parametre))
                {
                    resultat = resultat.Where(t => t.IdTableau.ToString() == parametre || t.TitreTableau == parametre);
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
