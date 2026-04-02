using System.Collections.Generic;
using System.Linq;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_TableauRepository
    {
        private readonly ProjectDbContext _context;

        public DTO_TableauRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public List<DTO_Tableau> GetAll()
        {
            return _context.Tableaux.ToList();
        }
    }
}
