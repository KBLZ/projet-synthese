using System.Collections.Generic;
using System.Linq;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_DescriptionRepository
    {
        private readonly ProjectDbContext _context;

        public DTO_DescriptionRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public List<DTO_Description> GetByTableauId(int tableauId)
        {
            return _context.Descriptions
                .Where(d => d.IdTableau == tableauId)
                .OrderBy(d => d.Position)
                .ToList();
        }
    }
}
