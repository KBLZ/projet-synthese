namespace EF_API_DB_SRV_Entities;

public class SelectionConstants
{
    public static (int min, int max) GetMinMaxFromSelection(int selection)
    {
        int min = selection * 100;
        int max = min + 100;
        return (min, max);
    }   
}