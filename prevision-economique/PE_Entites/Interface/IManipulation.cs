namespace PrevisionEconomique.Entites.Interface;

public interface IManipulation
{
<<<<<<< HEAD
    IEnumerable<T> LireDonnees<T>(string? parametre = null);
=======
   T LireDonnees<T>(string query, Dictionary<string, object>? parameters = null);
>>>>>>> ec2d882f7b2ce5fec3d5b24ddcb42ddbf52a6740
}