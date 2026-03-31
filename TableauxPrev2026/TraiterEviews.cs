using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Collections;
using System.Data;
using System.IO;

namespace tableauxPrevisions.Classes
{
    class TraiterEviews
    {
        private string m_listeMnemonique="cont* or niveau* or pch* or spec* or tab*";
        //private string m_listeMnemonique = "*";
        private string m_FichierEviews;      
        private ArrayList m_arSeries;
        private string m_page = "Trimestriel";
        private string m_erreur = "";


        public TraiterEviews(string p_FichierEviews)
        {
            this.m_FichierEviews = p_FichierEviews;
            m_arSeries = new ArrayList();
            readSeries();
        }

        public TraiterEviews(string p_FichierEviews, string p_listeMnemoniques, string p_page)
        {
            this.m_FichierEviews = p_FichierEviews;
            this.m_listeMnemonique = p_listeMnemoniques;
            this.m_page = p_page;
            m_arSeries = new ArrayList();
            readSeries();
        }

        public ArrayList getArrSeries()
        {
            return m_arSeries;
        }



         /// <summary>
        /// lire les données à partir d'un fichier Eviews pour les pages Trimestriel et annuel
        /// </summary>
        public void readSeries()
        {
            readSeriesPage("TRIMESTRIEL");
            readSeriesPage("ANNUEL");

        }

        /// <summary>
        /// lire les données à partir d'un fichier Eviews.
        /// </summary>
        private void readSeriesPage(string p_page)
        {
            ReadDBEviews rdEviews = new ReadDBEviews(m_FichierEviews, m_listeMnemonique, p_page);
            char freq = 'A';
            if (p_page == "TRIMESTRIEL") freq = 'Q';
            if (rdEviews.getErreur() != "")
            {
                m_erreur += rdEviews.getErreur();
                return;
            }
            DataTable dtData = rdEviews.getData();
            ArrayList arMnem = rdEviews.getListeMnemo();

            int nbSeries = arMnem.Count;
            // premiere colonne pour les date
            int nbObs = dtData.Rows.Count;
            String premieredate = dtData.Rows[0].ItemArray[0].ToString();
            String DerniereDate = dtData.Rows[nbObs-1].ItemArray[0].ToString();
           
            string donnee = "";
                     
            BarProgression brprogres = new BarProgression();
            brprogres.ProgressValue = 0;
            brprogres.ProgressText = "Chargement des données à partir du workfile, En train de lire la page "+p_page;
            brprogres.ProgressText2 = "0";
            brprogres.Show();

            int nb = nbSeries;
            int jj = 1;       
            double p = 0;
            int r = 2;
            if (nb > 500)
            {
                r = 5;
            }
            if (nb > 1000)
            {
                r = 10;
            }
            if (nb > 2000)
            {
                r = 20;
            }
           
            // pour toutes les series.
            for (int i = 1; i <= nbSeries; i++)
            {
                string Mnem = (string)arMnem[i-1];
               // SortedDictionary<string, decimal> obs = new SortedDictionary<string, decimal>();
                float[] tabobs=new float[nbObs];
                
                for (int j = 0; j < nbObs; j++)
                {

                    donnee = dtData.Rows[j].ItemArray[i].ToString();
                    float val = 0;


                    if (donnee != "")
                    {

                        float.TryParse(dtData.Rows[j].ItemArray[i].ToString(), out val);
                        tabobs[j] = val;

                    }
                    else
                    {
                        tabobs[j] = float.NaN;
                    }


                }//j
                p = i *100 / nb;
                           
                if (i / r == jj)
                {
                    p = i *100/ nb;                   
                    brprogres.ActualiserPB(p);

                    jj = jj + 1;
                }

                Serie sr = new Serie();
                sr.ListeObservation = tabobs;
                sr.Frequence = freq;
                sr.Mnemonique = Mnem;
                sr.PremierePeriode = premieredate;
                sr.DernierePeriode = DerniereDate;
                m_arSeries.Add(sr);
            }// for i
            p = 100;
            brprogres.ActualiserPB(p);         
            brprogres.Close();
        }//read
               


    }
}
