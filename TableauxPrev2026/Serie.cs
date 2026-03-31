using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tableauxPrevisions
{
    /// <summary>
    /// Classe représentant les données contenue d'un fichier prn ou excel
    /// </summary>
    public class Serie
    {
        /// <summary>
        /// constructeur par default
        /// </summary>
        public Serie()
        {
            m_ID = "";
            m_mnemonique = "";
            m_description = "";
            m_source = "";
            m_derniere_MAJ = "";
            m_premiere_periode = "2100-01-01";
            m_derniere_periode = "1900-01-01";
            m_frequence= ' ';
            m_Unite = "";
            //m_ListeObservation = null;
            //m_ListeObservation =new SortedDictionary<string, decimal>();
           
        }

        /// <summary>
        /// Propriété pour la donnée membre m_mnemonique
        /// </summary>
        public string Mnemonique
        {
            get { return m_mnemonique; }
            set { m_mnemonique = value; }
        }

        /// <summary>
        /// Propriété pour la donnée membre m_description
        /// </summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }
        
        /// <summary>
        /// Propriété pour la donnée membre m_source
        /// </summary>
        public string Source
        {
            get { return m_source; }
            set { m_source = value; }
        }
        
        /// <summary>
        /// Propriété pour la donnée membre m_derniere_MAJ
        /// </summary>
        public string DerniereMAJ
        {
            get { return m_derniere_MAJ; }
            set { m_derniere_MAJ = value; }
        }
        
        /// <summary>
        /// Propriété pour la donnée membre m_premiere_periode
        /// </summary>
        public string PremierePeriode
        {
            get { return m_premiere_periode; }
            set { m_premiere_periode = value; }
        }
        
        /// <summary>
        /// Propriété pour la donnée membre m_derniere_periode
        /// </summary>
        public string DernierePeriode
        {
            get { return m_derniere_periode; }
            set { m_derniere_periode = value; }
        }
        
        /// <summary>
        /// Propriété pour la donnée membre m_frequence
        /// </summary>
        public char Frequence
        {
            get { return m_frequence; }
            set { m_frequence = value; }
        }
        
        /// <summary>
        /// Propriété pour la donnée membre m_donne
        /// </summary>
        public float[] ListeObservation
        {
            get { return m_ListeObservation; }
            set { m_ListeObservation = value; }
        }

        /// <summary>
        /// Ajouter une observation dans la série.
        /// </summary>
        /// <param name="dateObservation">la date de l'observation</param>
        /// <param name="valeur">la valeur associé</param>
        //public void AddObservation(string dateObservation, Decimal valeur)
        //{
        //    m_ListeObservation.Add(dateObservation, valeur);
        //}

        /// <summary>
        /// le nom de la banque
        /// </summary>
        public string Banque
        {
            get { return m_banque; }
            set { m_banque = value; }
        }
        /// <summary>
        /// le scenario de la prevision
        /// </summary>
        public string Unite
        {
            get { return m_Unite; }
            set { m_Unite = value; }
        }

        /// <summary>
        /// ID de la serie dans la BD
        /// </summary>
        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }


        /// <summary>
        /// Retourner le premier element du dict.
        /// </summary>
        /// <param name="dict"></param>
        /// <remarks></remarks>
        //public string getFirstDateObs()
        //{
        //    //KeyValuePair<string, decimal> pair = default(KeyValuePair<string, decimal>);
        //    String firstDate = "2100-01-01";
        //    String first = m_ListeObservation.Keys.First();
        //    foreach (KeyValuePair<string, decimal> pair in m_ListeObservation)
        //    {

        //        DateTime fdt = new DateTime();
        //        fdt = Convert.ToDateTime(firstDate);
        //        DateTime Kdt = new DateTime();
        //        Kdt = Convert.ToDateTime(pair.Key);

        //        if (fdt > Kdt)
        //        {
        //            firstDate = pair.Key;
        //        }
        //    }
        //    return firstDate;
        //}


        //public decimal getObs(string dt)
        //{
        //    return m_ListeObservation[dt];
        //}
        /// <summary>
        /// Override +
        /// </summary>
        /// <param name="s1">Serie1</param>
        /// <param name="s2">Serie2</param>
        /// <returns></returns>
        //public static Serie operator +(Serie s1, Serie s2)
        //{
           
        //    Serie sr = new Serie();
        //    sr.m_banque = s1.Banque;
        //    sr.Mnemonique = s1.Mnemonique +"+"+ s2.Mnemonique;
        //    if (s1.Frequence.CompareTo(s2.Frequence) != 0)
        //    {

        //        sr.Description = "Les 2 series doivent avoir la même fréquence pour calculer leur somme.";
        //        return sr;
        //    }
        //    sr.Description =s1.Mnemonique+"+"+s2.Mnemonique+ " "+ s1.Description + s2.Description;
        //    sr.Frequence = s1.Frequence;

        //    if (s1.PremierePeriode.CompareTo(s2.PremierePeriode)>=0) sr.PremierePeriode = s1.PremierePeriode;
        //    else sr.PremierePeriode = s2.PremierePeriode;

        //    if (s1.DernierePeriode.CompareTo(s2.DernierePeriode) >= 0) sr.DernierePeriode = s2.DernierePeriode;
        //    else sr.DernierePeriode = s1.DernierePeriode;

        //    if (s1.DerniereMAJ.CompareTo(s2.DerniereMAJ) >= 0) sr.DerniereMAJ = s1.DerniereMAJ;
        //    else sr.DerniereMAJ = s2.DerniereMAJ;

        //    foreach (KeyValuePair<string, decimal> pair in s1.ListeObservation)
        //    {
        //        try
        //        {

        //            sr.ListeObservation.Add(pair.Key, pair.Value + s2.ListeObservation[pair.Key]);
        //        }
        //        catch
        //        {
                
        //        }

        //    }

        //    return  sr;
        //}




        //donné membre
        private string m_ID;
        private string m_mnemonique;
        private string m_description;
        private string m_source;
        private string m_derniere_MAJ;
        private string m_premiere_periode;
        private string m_derniere_periode;
        private char m_frequence;
        private string m_banque;
        private string m_Unite;
        private float[] m_ListeObservation; 
       
    }
}
