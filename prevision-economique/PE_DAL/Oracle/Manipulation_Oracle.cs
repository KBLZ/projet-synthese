using PrevisionEconomique.Entites.Interface;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace PE_DAL
{
<<<<<<< HEAD
    public class ManipulationOracle : IManipulation
    {
        public IEnumerable<T> LireDonnees<T>(string? parametre = null)
        {
            throw new NotImplementedException();
=======
    private readonly string _connectionString;

    public Manipulation_Oracle(string connectionString)
    {
        _connectionString = connectionString;
    }

    public T LireDonnees<T>(string query, Dictionary<string, object>? parameters = null)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentException("Query cannot be null or empty", nameof(query));
        }

        using (var connection = new OracleConnection(_connectionString))
        {
            connection.Open();
            using (var command = new OracleCommand(query, connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(new OracleParameter(param.Key, param.Value));
                    }
                }

                if (typeof(T) == typeof(DataTable))
                {
                    var dt = new DataTable();
                    using (var reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    return (T)(object)dt;
                }

                // Pour les autres types, on pourrait implémenter un mapping plus complexe
                // ou utiliser Dapper. Pour l'instant, on se limite au DataTable
                // comme base de données brute.
                throw new NotSupportedException($"Le mapping vers le type {typeof(T).Name} n'est pas encore implémenté.");
            }
>>>>>>> ec2d882f7b2ce5fec3d5b24ddcb42ddbf52a6740
        }
    }
}