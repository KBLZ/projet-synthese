 /// <summary>
        /// Constante pour le chemin du dossier Data
        /// </summary>
        public String REP_DATA = @"C:\programData\Prev_Tableaux\";        

        /// <summary>
        /// Constante pour le chemin du dossier Model
        /// </summary>
        /// M:\Rapport\FichierModele\PrevCMD
        public const String REP_MODEL = @"L:\FichierModele\PrevCMD\";
		
		 /// <summary>
        /// Méthode pour le bouton soumettre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Soumettre_Click(object sender, RoutedEventArgs e)
        {
            if (DeterminerExistenceFichier(txt_Banque1.Text) && (txt_Banque2.Visibility == Visibility.Visible && DeterminerExistenceFichier(txt_Banque2.Text) || txt_Banque2.Visibility == Visibility.Hidden))
            {
                String message = ValidationBanques(DeterminerNomsBanques(txt_Banque1.Text), DeterminerNomsBanques(txt_Banque2.Text));

                if (message != null)
                {
                    MessageBox.Show(message + ", veuillez sélectionner à nouveau à l'aide du bouton Parcourir", "Tableaux des prévisions", MessageBoxButton.OK, MessageBoxImage.Warning);
                }//if
                else
                {
                                
                    // c:\programdata\prevTableaux\prevcanwf.wf1 par exemple
                    String fichierWF = REP_DATA + configurationUtilisateur.ChoixWF + "wf.Wf1";
                    String fichierprg = REP_DATA + configurationUtilisateur.ChoixWF + "tmp.prg";
                    if (!Directory.Exists(REP_DATA)) Directory.CreateDirectory(REP_DATA);
                    // copier le workfile temporaire vers le repc
                    String fichierdst = REP_DATA  + configurationUtilisateur.ChoixWF + "tmp.wf1";
                    //if (!CopierFichier(fichiersrc, fichierdst))
                    //{
                    //    return;
                    //}
                    ArrayList arprg = new ArrayList();
                    arprg.Add("include " + (char)34 + REP_MODEL + configurationUtilisateur.ChoixWF + ".prg" + (char)34);
                    arprg.Add("'Les variables");
                    arprg.Add("%NomWF=" + (char)34 + txt_Banque1.Text + (char)34);
                    arprg.Add("%Dossier=" + (char)34 + REP_DATA + (char)34);

                    if (txt_Banque2.Text != null && configurationUtilisateur.ChoixWF.Contains("COMP"))
                    {
                        arprg.Add("%NomWF2=" + (char)34 + txt_Banque2.Text + (char)34);
                        //arprg.Add("call compcan(%NomWF,%NomWF2,%Dossier)");
                        arprg.Add("call " + configurationUtilisateur.ChoixWF + "(%NomWF,%NomWF2,%Dossier)");
                    }
                    else
                    { 
                        //arprg.Add("call prevcan(%NomWF,%Dossier)");
                        arprg.Add("call " + configurationUtilisateur.ChoixWF + "(%NomWF,%Dossier)");
                    
                    }
                                      
                    arprg.Add("EXIT");


                    WriteFilePRG(fichierprg, arprg);

                    ExeShellETattendFin(fichierprg);
                    File.Delete(fichierprg); // suprimer le fichier temporaire

                    /*********************************
                    * 
                    * 
                    * APPELLER LE PROCESSUS MFQ ICI
                    * 
                    * 
                    * *******************************/
                    configurationUtilisateur.URLBanque1 = txt_Banque1.Text;
                    configurationUtilisateur.URLBanque2 = txt_Banque2.Text;

                  //  String fichierPRN = REP_DATA + configurationUtilisateur.ChoixPRN + ".PRN";

                    if (DeterminerExistenceFichier(fichierWF))
                    {
                        TraiterEviews trEvs = new TraiterEviews(fichierWF);
                        contenuWF = trEvs.getArrSeries();
                        //ChargerPRN(fichierWF);
                        dateCreationWF = System.IO.File.GetCreationTime(fichierWF).ToLongDateString();

                        try
                        {
                            configurationUtilisateur.AnneeDebut = Convert.ToInt32(cmb_Annee.SelectedItem);
                        }//try
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erreur de conversion " + ex.ToString());
                            configurationUtilisateur.AnneeDebut = 0;
                        }//catch

                        //Affecter au curseur l'affichage traitement en arrière-plan.
                        Mouse.OverrideCursor = Cursors.Wait;

                        interfacePrincipale = new InterfacePrincipale(contenuWF, dateCreationWF, configurationUtilisateur, connexionBD);

                        //Masquer cette interface pour afficher l'interface principale.
                        Hide();
                        interfacePrincipale.ShowDialog();
                        interfacePrincipale.Close();
                        Close();
                    }//if
                }//else
            }//if
        }//btn_Soumettre_Click

        /// <summary>
        /// Copier un fichier vers une destination.
        /// </summary>
        /// <param name="src">Le nom du fichier source</param>
        /// <param name="dest">Le nom du fichier destination</param>
        /// <returns></returns>
        private Boolean CopierFichier(string src, string dest) {
            Boolean res = false;
            try
            {
                File.Copy(src, dest, true);
                res = true;
            }
            catch (Exception exp)
            {
                
                MessageBox.Show("On n'a pas pu copier le fichier " + src + " vers + " + dest+ "\r\n"+exp.ToString(), "Tableaux des prévisions", MessageBoxButton.OK, MessageBoxImage.Warning);
                
                res = false;
            }          
            return res;
        }



        /// <summary>
        /// Executer le programme Eviews 
        /// </summary>
        /// <param name="p_commande">La commande à exécuter</param>
        private void ExeShellETattendFin(string p_commande)
        {
            WaitCircular cirWait = new WaitCircular();
            cirWait.Show();
            int x1=(int)(this.Left + this.Width / 2 - cirWait.Width / 2);
            int y1=(int)(this.Top + this.Height / 2 - cirWait.Height / 2);
            cirWait.Location = new System.Drawing.Point(x1, y1);
                       
            cirWait.Refresh();

            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = p_commande;
                proc.Start();
                int v = 0;
                //on utilise double pour ralentir l'avancement            
                int x = 0;


                do
                {
                    // attendre la fin de la processus!
                    x = x + 1;
                    if (x == 1500)
                    {

                        v = v + 1;
                        cirWait.ActualiserCB(v);
                        x = 0;
                    }
                }
                while (!proc.HasExited);

            }
            catch (Exception exp)
            {
                MessageBox.Show("La commande " + p_commande + " n'a pas été executée!");
                MessageBox.Show(exp.ToString());
            }
            finally {

                cirWait.Close();
            }
        }


        /// <summary>
        /// Ecrire un programme eviews afin de l'exécuter
        /// </summary>
        /// <param name="DestFile"></param>
        public void WriteFilePRG(string DestFile, ArrayList p_lesLignes)
        {
            // dw pour le fichier destination
            StreamWriter sw = new StreamWriter(DestFile);

            //une ligne du fichier TSD
            foreach (string Ligne in p_lesLignes)
            {
                sw.WriteLine(Ligne);
            }

            sw.Close();
        }