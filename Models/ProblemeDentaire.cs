using System;
using Npgsql;
using System.Data.SqlClient;

namespace dentiste.Models;

public class ProblemeDenatire  {
    public int id { get; set; }
    public String date { get; set; }
    public int numero { get; set; }
    public String idPatient { get; set; }
    public int idDent { get; set; }
    public int probleme { get; set; }
    public int etat { get; set; }

    public ProblemeDenatire() {}

    public ProblemeDenatire(int id) {
        this.id = id;
    }

    public ProblemeDenatire(String date, String numero, String idPatient, String idDent, String probleme) {
        this.date = date;
        this.numero = int.Parse(numero);
        this.idPatient = idPatient;
        this.idDent = int.Parse(idDent);
        this.probleme = int.Parse(probleme);
    }



    public int getNextNumero() {
        Connexion connexion = new Connexion();
        int numero = 0;
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select coalesce(nextval(seqProbleme),1);", conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            numero = reader.GetInt32(0);
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
        return numero;
    }

    public void insert() {
        Connexion connexion = new Connexion();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                String commande = "insert into probleme_dentaire(date, numero, idpatient, iddent, probleme) values('"+this.date+"', "+this.numero+", '"+this.idPatient+"', "+this.idDent+", "+this.probleme+")";
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    cmd.ExecuteReader();
                } 
                conn.Close();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Erreur: {ex}"); 
        }
    }

    public void ajoutControle() {
        Connexion connexion = new Connexion();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = "insert into controle(idProbleme) values('"+this.id+"')";
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    cmd.ExecuteReader();
                } 
                conn.Close();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Erreur: {ex}"); 
        }
    }
    
    // public List<Patient> getListePatient(string id = "") {
    //     Connexion connexion = new Connexion();
    //     List<Patient> liste = new List<Patient>();
    //     try 
    //     {
    //         using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
    //         {
    //             conn.Open();
    //             string commande = "SELECT * FROM Patients order by id asc";
    //             if(id != "") 
    //                 commande = "SELECT * FROM Patients where id = '" + id + "'";
    //             Console.WriteLine(commande);
    //             using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
    //             {
    //                 using (NpgsqlDataReader reader = cmd.ExecuteReader())
    //                 {
    //                     while (reader.Read())
    //                     {
    //                         Patient patient = new Patient(reader.GetString(1),reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetString(5));
    //                         patient.id = reader.GetString(0);
    //                         liste.Add(patient);
    //                     }
    //                     reader.Close();
    //                 } 
    //             } 
    //             conn.Close();
    //         } 
    //     }
    //     catch (System.Exception ex)
    //     {
    //         Console.WriteLine($"Erreur: {ex}"); 
    //     }
    //     return liste;
    // }

    
}
