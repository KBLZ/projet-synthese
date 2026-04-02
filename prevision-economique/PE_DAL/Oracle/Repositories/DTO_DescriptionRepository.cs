using System.Collections.Generic;
using System.Data;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_DescriptionRepository
    {
        private readonly Manipulation_Oracle _manipulation;

        public DTO_DescriptionRepository(Manipulation_Oracle manipulation)
        {
            _manipulation = manipulation;
        }

        public List<DTO_Description> GetByTableauId(int tableauId)
        {
            var query = "SELECT IDTABLEAU, POSITION, NIVEAU, MNEMONIQUE, DESCRIPTION, LIGNE1_TAB, LIGNE3_NIV_SPEC, LIGNE4_PCH_CONT, VARIATION, DECIMALE, NOTE FROM TPF_DESCRIPTIONS WHERE IDTABLEAU = :tableauId ORDER BY POSITION";
            var parameters = new Dictionary<string, object> { { "tableauId", tableauId } };
            var dt = _manipulation.LireDonnees<DataTable>(query, parameters);
            var list = new List<DTO_Description>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DTO_Description
                {
                    IdTableau = Convert.ToInt32(row["IDTABLEAU"]),
                    Position = Convert.ToInt32(row["POSITION"]),
                    Niveau = Convert.ToInt32(row["NIVEAU"]),
                    Mnemonic = row["MNEMONIQUE"].ToString(),
                    DescriptionTexte = row["DESCRIPTION"].ToString(),
                    Ligne1Tableau = row["LIGNE1_TAB"].ToString(),
                    Ligne3NiveauSpec = row["LIGNE3_NIV_SPEC"].ToString(),
                    Ligne4PchCont = row["LIGNE4_PCH_CONT"].ToString(),
                    Variation = Convert.ToInt32(row["VARIATION"]),
                    Decimale = Convert.ToInt32(row["DECIMALE"]),
                    Note = row["NOTE"] == DBNull.Value ? 0 : Convert.ToInt32(row["NOTE"])
                });
            }
            return list;
        }
    }
}
