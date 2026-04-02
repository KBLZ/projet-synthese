using PE_DAL.Oracle.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_NoteRepository
    {


        private readonly PE_DBContext m_context;

        public DTO_NoteRepository(PE_DBContext context)
        {
            m_context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<T> LireDonnees<T>(string? parametre = null)
        {
            if (typeof(T) == typeof(DTO_Note))
            {
                IEnumerable<DTO_Note> resultat = m_context.Notes.AsEnumerable();

                if (!string.IsNullOrEmpty(parametre))
                {
                    resultat = resultat.Where(n =>n.IdNote.ToString() == parametre || n.TexteNote == parametre);
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
