using System.Collections.Generic;
using System.Linq;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_HistoriqueRepository
    {
        private readonly ProjectDbContext _context;

        public DTO_HistoriqueRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public List<DTO_Historique> GetByUserId(string userId)
        {
            return _context.Historiques
                .Where(h => h.IdUtilisateur == userId)
                .ToList();
        }
    }
}
