namespace PrevisionEconomique.Entites.Interface;

public interface IManipulation
{
   T LireDonnees<T>(string query, Dictionary<string, object>? parameters = null);
}