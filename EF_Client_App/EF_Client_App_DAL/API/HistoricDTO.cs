using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = EF_Client_App_Entity;

namespace EF_Client_App_DAL
{

    public class HistoricDTO
    {
        public string UserId { get; init; } = string.Empty;
        public string PRN_Selection { get; init; } = string.Empty;
        public string UrlPool1 { get; init; } = string.Empty;
        public string UrlPool2 { get; init; } = string.Empty;

        public int StartedYear { get; init; }
        public int StartedQuarter { get; init; }
        public int IndexTitleTab { get; set; }
        public int DisplayMode { get; set; }

        public HistoricDTO()
        {
            ;
        }

        public HistoricDTO(
            string userId,
            string prnSelection,
            string urlPool1,
            string urlPool2,
            int startedYear,
            int startedQuarter,
            int indexTitleTab,
            int displayMode
        )
        {
            UserId = userId;
            PRN_Selection = prnSelection;
            UrlPool1 = urlPool1;
            UrlPool2 = urlPool2;
            StartedYear = startedYear;
            StartedQuarter = startedQuarter;
            IndexTitleTab = indexTitleTab;
            DisplayMode = displayMode;
        }

        public Entity.Historic ToEntity()
        {
            return new Entity.Historic(
                this.UserId,
                this.PRN_Selection,
                this.UrlPool1,
                this.UrlPool2,
                this.StartedYear,
                this.StartedQuarter,
                this.IndexTitleTab,
                this.DisplayMode
               
            );
        }

        public override string ToString()
        {
            return $"UserId: {this.UserId}, " +
                   $"PRN_Selection: {this.PRN_Selection}, " +
                   $"UrlPool1: {this.UrlPool1}, " +
                   $"UrlPool2: {this.UrlPool2}, " +
                   $"StartedYear: {this.StartedYear}, " +
                   $"StartedQuarter: {this.StartedQuarter}, " +
                   $"IndexTitleTab: {this.IndexTitleTab}, " +
                   $"DisplayMode: {this.DisplayMode}";
        }
    }
}