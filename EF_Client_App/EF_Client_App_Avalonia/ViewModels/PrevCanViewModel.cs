using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class PrevCanViewModel : ViewModelBase
    {
        // CTOR
        public PrevCanViewModel(MainWindowViewModel mainVM)
        {
            mainVM.TypeRecherche = "Prévisions du Canada";
            mainVM.Tableau = "Tableau ABC";
            mainVM.Banque1 = "Banque 1";
            mainVM.Banque2 = "(une seule pour les prévisions)";
            mainVM.IsAccueil = false;
        }
    }
}
