using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Globalization;

namespace tableauxPrevisions
{
    class ReadDBEviews
    {

        //private DataSet ds;
        private DataTable dtData;
        private DataTable dtObjets;
        private string EviewsFileName;
        private string listeSeries;
        private string m_page;
        private string m_Erreur = "";
        private ArrayList m_arrMnem;
        private int nbSeries = 0;
        private int nbObs = 0;

        private OleDbConnection conn;

        /// <summary>
        ///  constructeur vide
        /// </summary>
        public ReadDBEviews()
        {
        }

        /// <summary>
        /// Lire un fichier Eviews 
        /// </summary>
        /// <param name="p_nomFichier">nom du fichier Eviews</param>
        /// <param name="p_listeSeries">une chaine de caractères contenant la liste de series à chercher.</param>
        /// <param name="p_page">la page à lire.
        /// Ex. *_M ou bien serie1 serie2 serie3</param>
        public ReadDBEviews(string p_nomFichier, string p_listeSeries, string p_page)
        {
            EviewsFileName = p_nomFichier;
            listeSeries = p_listeSeries;
            m_page = p_page;
            ListObjects();
            LireDonnees();
        }
        /// <summary>
        /// retourne une dataTable contenant les données de toutes les series
        /// </summary>
        /// <returns></returns>
        public DataTable getData()
        {
            return dtData;
        }

        /// <summary>
        /// Lire une autre page du workfile 
        /// retourne une dataTable contenant les données de toutes les series
        /// </summary>
        /// <param name="p_listeSeries">une chaine de caractères contenant la liste de series à chercher.</param>
        /// <param name="p_page">la page à lire.</param>
        /// <returns></returns>
        public DataTable ReadData(string p_listeSeries, string p_page)
        {
            listeSeries = p_listeSeries;
            m_page = p_page;
            ListObjects();
            LireDonnees();
            return dtData;
        }


        /// <summary>
        /// retourne une dataTable contenant les données de toutes les series
        /// </summary>
        /// <returns></returns>
        public String getErreur()
        {
            return m_Erreur;
        }


        /// <summary>
        /// retourne une dataTable contenant les objets (series) et leurs description, units, start, end,.. du BD Eviews
        /// </summary>
        /// <returns></returns>
        public DataTable getObjets()
        {
            return dtObjets;
        }

        /// <summary>
        /// Retourne la liste de toutes les mnemoniques.
        /// </summary>
        /// <returns></returns>
        public ArrayList getListeMnemo()
        {
            return m_arrMnem;
        }



        /// <summary>
        /// retourne un arraylist de toutes les donnees de toutes les series
        /// c'est une arraylist de arraylist.
        /// </summary>       
        /// <param name="nbMois">nombre des mois</param>
        /// <returns></returns>
        public ArrayList getListeData(int nbMois)
        {
            ArrayList arArData = new ArrayList();
            // indice de la premiere donnee dans la datatable
            int ind = 0;// DiffMois(debut, getDateDebut());
            string donnee;

            // création d'une arraylist avec des valeurs NAN.
            //ArrayList arrNAN = new ArrayList(nbMois);


            // pour toutes les series.
            for (int i = 0; i < nbSeries; i++)
            {
                ArrayList ar = new ArrayList();
                // On remplit la liste de données avec des missings
                for (int k = 0; k < nbMois; k++)
                {
                    ar.Add(Single.NaN);
                }

                for (int j = ind; j < nbObs; j++)
                {
                    donnee = dtData.Rows[j].ItemArray[i + 1].ToString();

                    if (donnee != "")
                    {
                        ar[j] = String2Single(donnee);
                    }


                }//j
                arArData.Add(ar);

            }// for i


            return arArData;
        }

        private void connecter()
        {
                conn=new OleDbConnection();           
                try
                {
                    string stconn = "Provider= EViewsOleDbProvider.EViewsProv;";
                    if (getExtention() == "WF1")
                    {
                       stconn += "Initial Catalog=" + m_page + ";";
                    }

                    stconn+= " Path=" + EviewsFileName +"; Include ID Series=false";
                    conn.ConnectionString = stconn;
                    conn.Open();
                }
                catch(Exception exp)
                {
                    string ster = "Erreur :" + exp.ToString();
                    m_Erreur += ster;

                }
        }


        /// <summary>
        /// Lire les données de toutes les series.
        /// les series sont dans une liste ou bien avec un paterns comme *_M
        /// </summary>
        private void LireDonnees()
        {
            connecter();

            try
            {

                dtData = new DataTable();

                string formatdate = "@strdate(\"yyyy-MM-DD\")  ";
                // tester listmnem = "PIB PIB8";
                string listmnem = "";
                foreach (string str in m_arrMnem)
                {
                    listmnem += str + " ";
                }

                OleDbDataAdapter adpt = new OleDbDataAdapter(formatdate + listmnem, conn);

                adpt.Fill(dtData);

            }
            catch (Exception exp)
            {
                // mbox("Erreur :" + exp.ToString());
                string st = "Erreur :" + exp.ToString();
                m_Erreur += st;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State==ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }// endtry

        }//liredonnes

        private string getExtention()
        {
			
            string ext = System.IO.Path.GetExtension(EviewsFileName).toUpper();
            //string[] tabName = EviewsFileName.Split('.');
            //ext = tabName[tabName.Length-1].ToUpper();

            return ext;
        }


         /// <summary>
        /// Chercher les series (nom, type, units,description, de la BD Eviews
        /// </summary>
        private ArrayList ListObjects()
        {

            nbSeries = 0;
            m_arrMnem = new ArrayList();
            ListObjectsPartiel(listeSeries);

            nbSeries = dtObjets.Rows.Count;

            string mnem;
            for (int j = 0; j < nbSeries; j++)
            {
                mnem = dtObjets.Rows[j].ItemArray[0].ToString();
                m_arrMnem.Add(mnem);
            }

            return m_arrMnem;
        }
        

        /// <summary>
        /// Chercher les series (nom, type, units,description, de la BD Eviews
        /// </summary>
        private void ListObjectsPartiel(string p_pattern)
        {          
            
            try
            {
                connecter();
                dtObjets = new DataTable();
                string cmdsql = "<meta> select name, type,units,description,start,end where name matches " + (char)34+ p_pattern +(char)34+ " order by name";
                OleDbDataAdapter adpt = new OleDbDataAdapter(cmdsql, conn);
                adpt.Fill(dtObjets);
             

            }
            catch (Exception exp)
            {

                m_Erreur = "Erreur :" + exp.ToString();
                nbSeries = 0;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }// endtry


        }// fin de objets

        /// <summary>
        /// Convertir une chaine vers un single en prenant compte de la virgule( . ou ,)
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private Single String2Single(string val)
        {
            Single dat = Single.NaN;
            CultureInfo cu = CultureInfo.CurrentCulture;
            if (cu.NumberFormat.NumberDecimalSeparator == ",")
                val = val.Replace('.', ',');
            Single.TryParse(val, out dat);
            return dat;

        }//st2single

        /// <summary>
        /// convertir une chaine vers une date
        /// format de la chaine: YYYY-MM-DD
        /// </summary>
        /// <param name="dt">YYYY-MM-DD</param>
        /// <returns></returns>
        private DateTime StringToDate(String dt)
        {
            DateTime dt1 = DateTime.Now;
            try
            {
                //1950-01-01
                //0123456789
                int an = Convert.ToInt16(dt.Substring(0, 4));
                int mois = Convert.ToInt16(dt.Substring(5, 2));
                int jour = Convert.ToInt16(dt.Substring(8, 2));
                dt1 = new DateTime(an, mois, jour);
            }
            catch (Exception ex)
            {
                m_Erreur += ex.ToString();
            }
            return dt1;
        }
    }
}
