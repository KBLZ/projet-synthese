using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class CompQcViewModel : ViewModelBase
    {
        // CTOR
        public CompQcViewModel(MainWindowViewModel mainVM)
        {
            mainVM.TypeRecherche = "Comparaison des prévisions du Québec";
            mainVM.Tableau = "Tableau XYZ";
            mainVM.Banque1 = "Banque 1";
            mainVM.Banque2 = "Banque 2";
            mainVM.IsAccueil = false;
        }
    }
}
