using System.Collections.Generic;
using System.Linq;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_NoteRepository
    {
        private readonly ProjectDbContext _context;

        public DTO_NoteRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public List<DTO_Note> GetAll()
        {
            return _context.Notes.ToList();
        }
    }
}
