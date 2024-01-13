
using System;
using Npgsql;

namespace dentiste.Models;

public class Connexion {
    public NpgsqlConnection connection;

    public Connexion(){
        string connectionString = "Host=localhost;UserName=postgres;Password=fanatenana;Database=dentiste";
        connection = new NpgsqlConnection(connectionString);
    }

    public NpgsqlConnection connexion {
       get; set;
    }
        
    
}
