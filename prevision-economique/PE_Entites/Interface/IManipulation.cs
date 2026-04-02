namespace PrevisionEconomique.Entites.Interface;

public interface IManipulation
{
    IEnumerable<T> LireDonnees<T>(string? parametre = null);
}