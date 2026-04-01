namespace PrevisionEconomique.Entites.Interface;

public interface IManipulation
{
   T LireDonnees<T>(string parametre = null);
}