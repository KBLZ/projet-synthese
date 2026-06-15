using EF_Client_App_Entity.Enums;

namespace EF_Client_App_Entity.Records;

public record ConfigurationImport(
    DataTypes DataType,
    GovernmentLevel GovernmentLevel
    );