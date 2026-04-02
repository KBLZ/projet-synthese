using System.Collections.Generic;
using System.Data;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_TableauRepository
    {
        private readonly Manipulation_Oracle _manipulation;

        public DTO_TableauRepository(Manipulation_Oracle manipulation)
        {
            _manipulation = manipulation;
        }

        public List<DTO_Tableau> GetAll()
        {
            var dt = _manipulation.LireDonnees<DataTable>("SELECT IDTABLEAU, TITRETABLEAU, SOUSTITRETABLEAU FROM TPF_TABLEAUX");
            var list = new List<DTO_Tableau>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DTO_Tableau
                {
                    IdTableau = Convert.ToInt32(row["IDTABLEAU"]),
                    TitreTableau = row["TITRETABLEAU"].ToString(),
                    SousTitreTableau = row["SOUSTITRETABLEAU"] == DBNull.Value ? null : row["SOUSTITRETABLEAU"].ToString()
                });
            }
            return list;
        }
    }
}
