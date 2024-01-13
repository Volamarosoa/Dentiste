using System;
using Npgsql;
using System.Data.SqlClient;

namespace dentiste.Models;

public class Dent  {
    public int id { get; set; }
    public string nom { get; set; }
    public int type { get; set; }
    public int etat { get; set; }

    public Dent() {}

    public Dent(int id, String nom, int type, int etat) {
        this.id = id;
        this.nom = nom;
        this.type = type;
        this.etat = etat;
    }

    public void setIdDent() {
        Connexion connexion = new Connexion();
        int next = 0;
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select nextSeqParseil();", conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            next = (int)reader.GetInt32(0);
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

    // public void insert() {
    //     Connexion connexion = new Connexion();
    //     try 
    //     {
    //         using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
    //         {
    //             conn.Open();
    //             string commande = "insert into parseil values('"+this.idParseil+"', '"+this.idResponsable+"', '"+this.nom+"')";
    //             Console.WriteLine(commande);
    //             using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
    //             {
    //                 cmd.ExecuteReader();
    //             } 
    //             conn.Close();
    //         }
    //     }
    //     catch (System.Exception ex)
    //     {
    //         Console.WriteLine($"Erreur: {ex}"); 
    //     }
    // }
    
    public List<Dent> getListeDent() {
        Connexion connexion = new Connexion();
        List<Dent> liste = new List<Dent>();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = "SELECT * FROM Dents order by id asc"; 
                if(this.id !=0 )
                    commande = "SELECT * FROM Dents where id = "+this.id;
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dent Dent = new Dent(reader.GetInt32(0),reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3));
                            liste.Add(Dent);
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
        return liste;
    }

    public int getType() {
        Connexion connexion = new Connexion();
        int type = 0;
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = "select * from type_etat where idetat = " + this.etat; 
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dent Dent = new Dent(reader.GetInt32(0),reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3));
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
        return type;
    }

    public double getPrix() {
        return 0;
    }



    
}
