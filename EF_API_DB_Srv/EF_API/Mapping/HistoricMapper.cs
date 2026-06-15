using EF_API_DB_Srv_DAL.Oracle.DTO;
using Model = EF_API.Models;

namespace EF_API.Mapping;

public class HistoricMapper
{
    public Model.Historic ToModel(DTO_Historic p_DTOHistoric)
    {
        if (p_DTOHistoric == null) return new Model.Historic();

        return new Model.Historic
        {
            UserId = p_DTOHistoric.UserId,
            PRN_Selection = p_DTOHistoric.PRN_Selection,
            UrlPool1 = p_DTOHistoric.UrlPool1,
            UrlPool2 = p_DTOHistoric.UrlPool2,
            StartedYear = p_DTOHistoric.StartedYear,
            StartedQuarter = p_DTOHistoric.StartedQuarter,
            IndexTitleTab = p_DTOHistoric.IndexTitleTab,
            DisplayMode = p_DTOHistoric.DisplayMode
        };
    }

    public DTO_Historic ToDTO(Model.Historic p_historicModel)
    {
        if (p_historicModel == null) return new DTO_Historic();

        return new DTO_Historic
        {
            UserId = p_historicModel.UserId,
            PRN_Selection = p_historicModel.PRN_Selection,
            UrlPool1 = p_historicModel.UrlPool1,
            UrlPool2 = p_historicModel.UrlPool2,
            StartedYear = p_historicModel.StartedYear,
            StartedQuarter = p_historicModel.StartedQuarter,
            IndexTitleTab = p_historicModel.IndexTitleTab,
            DisplayMode = p_historicModel.DisplayMode
        };
    }
}