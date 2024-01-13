using System;
using Npgsql;
using System.Data.SqlClient;

namespace dentiste.Models;

public class Type  {
    public int id { get; set; }
    public int min { get; set; }
    public int max { get; set; }
    public double prix { get; set; }

    public Type() {}

    public Type(int id) {
        this.id = id;
    }

    public Type(int id, int min, int max, double prix) {
        this.id = id;
        this.min = min;
        this.max = max;
        this.prix = prix;
    }

    
    public void setType(int etat) {
        Connexion connexion = new Connexion();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = "select * from type_etat where idetat = "+ etat +" and isValid = 1";
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        this.id = reader.GetInt32(1);
                        }
                        reader.Close();
                    } 
                } 
                conn.Close();
            } 
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Erreur: {ex}"); 
        }
    }
    
    public void setMinMax() {
        Connexion connexion = new Connexion();
        List<int> values = new List<int>();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = "select * from type_etat where idtype = "+ this.id +" and isValid = 1 order by idEtat desc";
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            values.Add(reader.GetInt32(2));
                        }
                        reader.Close();
                    } 
                } 
                conn.Close();
            } 
            if(values.Count() > 0) {
                this.max = values[0];
                this.min = values[values.Count() - 1];
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Erreur: {ex}"); 
        }
    }

    public void setPrix(String date, int idDent) {
        Connexion connexion = new Connexion();
        this.prix = 0;
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = " select * from prix where date <= '"+ date +"' and idDent = "+ idDent +" and idType = "+ this.id +" order by date desc limit 1";
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.prix = reader.GetDouble(4);
                        }
                        reader.Close();
                    } 
                } 
                conn.Close();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Erreur: {ex}"); 
        }
    }

    public Type getType(String date, int idDent, int etat) {
        Type type = new Type();
        type.setType(etat);
        type.setMinMax();
        type.setPrix(date, idDent);
        return type;
    }
}
