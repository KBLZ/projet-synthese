using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public class AccueilViewModel : ViewModelBase
    {        public AccueilViewModel(MainWindowViewModel mainVM)
        {
            mainVM.TypeRecherche = "Prévisions économiques";
            mainVM.Tableau = Environment.UserName;
            mainVM.Banque1 = "";
            mainVM.Banque2 = "";
            mainVM.IsAccueil = true;
        }
    }
}
