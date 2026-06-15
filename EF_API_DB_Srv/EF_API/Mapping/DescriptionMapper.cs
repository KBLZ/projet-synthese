using EF_API_DB_Srv_DAL.Oracle.DTO;
using Model = EF_API.Models;

namespace EF_API.Mapping;

public class DescriptionMapper
{
    public Model.Description ToModel(DTO_Description p_DTODescription)
    {
        if (p_DTODescription == null) return new Model.Description();

        return new Model.Description
        {
            ArrayId = p_DTODescription.ArrayId,
            Position = p_DTODescription.Position,
            Level = p_DTODescription.Level,
            Mnemonic = p_DTODescription.Mnemonic,
            TextDescription = p_DTODescription.TextDescription,
            FirstLineArray = p_DTODescription.FirstLineArray,
            Line3LevelSpec = p_DTODescription.Line3LevelSpec,
            Line4PchCont = p_DTODescription.Line4PchCont,
            Variation = p_DTODescription.Variation,
            Decimal = p_DTODescription.Decimal,
            Note = p_DTODescription.Note
        };
    }

    public DTO_Description ToDTO(Model.Description p_descriptionModel)
    {
        if (p_descriptionModel == null) return new DTO_Description();

        return new DTO_Description
        {
            ArrayId = p_descriptionModel.ArrayId,
            Position = p_descriptionModel.Position,
            Level = p_descriptionModel.Level,
            Mnemonic = p_descriptionModel.Mnemonic,
            TextDescription = p_descriptionModel.TextDescription,
            FirstLineArray = p_descriptionModel.FirstLineArray,
            Line3LevelSpec = p_descriptionModel.Line3LevelSpec,
            Line4PchCont = p_descriptionModel.Line4PchCont,
            Variation = p_descriptionModel.Variation,
            Decimal = p_descriptionModel.Decimal,
            Note = p_descriptionModel.Note
        };
    }
}