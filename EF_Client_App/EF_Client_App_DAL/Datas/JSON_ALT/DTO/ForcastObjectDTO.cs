namespace EF_Client_App_DAL.JSON.DTO;

public record ForcastObjectDTO(
    string Name,
    string DataType,
    string ObjectType,
    ObservationPointDTO ObsMax,
    ObservationPointDTO ObsMin,
    UpdateDTO LastUpdate,
    double[] Values,
    string? ConvertHilo,
    string? ConvertLohi
);