using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class CompCanViewModel : ViewModelBase
    {
        // CTOR
        public CompCanViewModel(MainWindowViewModel mainVM)
        {
            mainVM.TypeRecherche = "Comparaison des prévisions du Canada";
            mainVM.Tableau = "Tableau ABC";
            mainVM.Banque1 = "Banque 1";
            mainVM.Banque2 = "Banque 2";
            mainVM.IsAccueil = false;
        }
    }
}