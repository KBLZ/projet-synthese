using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class PrevQcViewModel : ViewModelBase
    {
        // CTOR
        public PrevQcViewModel(MainWindowViewModel mainVM)
        {
            mainVM.TypeRecherche = "Prévisions du Québec";
            mainVM.Tableau = "Tableau XYZ";
            mainVM.Banque1 = "Banque 1";
            mainVM.Banque2 = "(une seule pour les prévisions)";
            mainVM.IsAccueil = false;
        }
    }
}
