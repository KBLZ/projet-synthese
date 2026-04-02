using Microsoft.EntityFrameworkCore;
using PE_DAL.Oracle.Context;
using PrevisionEconomique.Entites.Interface;


namespace PE_DAL.Oracle.Repositories
{
    public class DTO_DescriptionRepository : IManipulation, IDisposable
    {

        private readonly PE_DBContext m_context;

        public DTO_DescriptionRepository(PE_DBContext context)
        {
            m_context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<T> LireDonnees<T>(string? parametre = null)
        {
            if (typeof(T) == typeof(DTO_Description))
            {
                IEnumerable<DTO_Description> resultat = m_context.Descriptions.AsEnumerable();

                if (!string.IsNullOrEmpty(parametre))
                {
                    resultat = resultat.Where(d => d.DescriptionTexte == parametre || d.Ligne1Tableau == parametre);
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
