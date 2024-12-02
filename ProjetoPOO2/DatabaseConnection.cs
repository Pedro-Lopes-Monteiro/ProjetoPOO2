using System;
using MySql.Data.MySqlClient;

public class DatabaseConnection
{
    private static DatabaseConnection _instance;
    private MySqlConnection _connection;
    private string _connectionString;

    // O construtor é privado para garantir que a instância seja única
    private DatabaseConnection()
    {
        _connectionString = "Server=127.0.0.1; Database=SistemaEstoque; Uid=root; Pwd=123456;";
        _connection = new MySqlConnection(_connectionString);
    }

    // Método para obter a instância única
    public static DatabaseConnection GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DatabaseConnection();
        }
        return _instance;
    }

    // Método para abrir a conexão
    public void OpenConnection()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            _connection.Open();
        }
    }

    // Método para fechar a conexão
    public void CloseConnection()
    {
        if (_connection.State != System.Data.ConnectionState.Closed)
        {
            _connection.Close();
        }
    }

    // Propriedade para obter a conexão
    public MySqlConnection Connection
    {
        get { return _connection; }
    }
}
