using EF_API_DB_Srv_DAL.Oracle.DTO;
using Model = EF_API.Models;

namespace EF_API.Mapping;

public class ArrayMapper
{
    public Model.Array ToModel(DTO_Array p_DTOArray)
    {
        if (p_DTOArray == null) return new Model.Array();

        return new Model.Array
        {
            ArrayId = p_DTOArray.ArrayId,
            Title = p_DTOArray.Title,
            SubTitle = p_DTOArray.SubTitle
        };
    }

    public DTO_Array ToDTO(Model.Array p_arrayModel)
    {
        if (p_arrayModel == null) return new DTO_Array();


        return new DTO_Array
        {

            /*
            ArrayId = p_arrayModel.ArrayId;
            Title = p_arrayModel.Title;
            SubTitle = p_arrayModel.SubTitle;
            */
        };

    }

}