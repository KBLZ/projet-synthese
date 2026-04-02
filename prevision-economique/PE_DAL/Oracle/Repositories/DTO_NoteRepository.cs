using System.Collections.Generic;
using System.Data;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_NoteRepository
    {
        private readonly Manipulation_Oracle _manipulation;

        public DTO_NoteRepository(Manipulation_Oracle manipulation)
        {
            _manipulation = manipulation;
        }

        public List<DTO_Note> GetAll()
        {
            var dt = _manipulation.LireDonnees<DataTable>("SELECT IDNOTE, TEXTENOTE FROM TPF_NOTE");
            var list = new List<DTO_Note>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DTO_Note
                {
                    IdNote = Convert.ToInt32(row["IDNOTE"]),
                    TexteNote = row["TEXTENOTE"].ToString()
                });
            }
            return list;
        }
    }
}
