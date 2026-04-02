using System.Collections.Generic;
using System.Data;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_HistoriqueRepository
    {
        private readonly Manipulation_Oracle _manipulation;

        public DTO_HistoriqueRepository(Manipulation_Oracle manipulation)
        {
            _manipulation = manipulation;
        }

        public List<DTO_Historique> GetByUserId(string userId)
        {
            var query = "SELECT ID_UTILISATEUR, CHOIXPRN, URL_Banque1, URL_Banque2, AnneeDebut, TrimDebut, IndexTitreTab, ModeAffichage FROM TPF_HISTO_UTILISATEUR WHERE ID_UTILISATEUR = :userId";
            var parameters = new Dictionary<string, object> { { "userId", userId } };
            var dt = _manipulation.LireDonnees<DataTable>(query, parameters);
            var list = new List<DTO_Historique>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DTO_Historique(
                    row["ID_UTILISATEUR"].ToString(),
                    row["CHOIXPRN"].ToString(),
                    row["URL_Banque1"].ToString(),
                    row["URL_Banque2"] == DBNull.Value ? string.Empty : row["URL_Banque2"].ToString(),
                    Convert.ToInt32(row["AnneeDebut"]),
                    Convert.ToInt32(row["TrimDebut"]),
                    Convert.ToInt32(row["IndexTitreTab"]),
                    Convert.ToInt32(row["ModeAffichage"])
                ));
            }
            return list;
        }
    }
}
