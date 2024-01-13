using System;
using Npgsql;
using System.Data.SqlClient;

namespace dentiste.Models;

public class Patient  {
    public String id { get; set; }
    public string nom { get; set; }
    public string prenom { get; set; }
    public string date_de_naissance { get; set; }
    public int idGenre { get; set; }
    public string date { get; set; }
    public List<EtatDentaire> etatDentaire { get; set; }

    public double aPayer { get; set; }
    public double reste { get; set; }

    public Patient() {}

    public Patient(String id, String date) {
        this.id = id;
        this.date = date;
    }

    public Patient(String nom, String prenom, String date_de_naissance, int idGenre, String date) {
        this.nom = nom;
        this.prenom = prenom;
        this.date_de_naissance = date_de_naissance;
        this.idGenre = idGenre;
        this.date = date;
    }

    public String GetGenre() {
        if(this.idGenre == 1)
            return "Homme";
        return "Femme";
    }


    public void setIdPatient() {
        Connexion connexion = new Connexion();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select nextID( 'seqPatient', 'PT', 10);", conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.id = reader.GetString(0);
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

    public void insert() {
        Connexion connexion = new Connexion();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = "insert into patients values('"+this.id+"', '"+this.nom+"', '"+this.prenom+"', '"+this.date_de_naissance+"', '"+this.idGenre+"', '"+this.date+"')";
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
    
    public List<Patient> getListePatient() {
        Connexion connexion = new Connexion();
        List<Patient> liste = new List<Patient>();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = "SELECT * FROM Patients where date <= '"+ this.date +"' order by id asc";
                if(this.id != "") 
                    commande = "SELECT * FROM Patients where id = '" + id + "' and date <= '"+ this.date +"'";
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Patient patient = new Patient(reader.GetString(1),reader.GetString(2), reader.GetDateTime(3).ToString("dd/MM/yyyy"), reader.GetInt32(4), reader.GetDateTime(5).ToString("dd/MM/yyyy"));
                            patient.id = reader.GetString(0);
                            liste.Add(patient);
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

    public Patient getPatient() {
        List<Patient> patients = this.getListePatient();
        if(patients.Count() > 0) {
            patients[0].etatDentaire = (new EtatDentaire(this.date, this.id)).getEtatDentaire();
            return patients[0];
        }
        return null;
    }

    public List<EtatDentaire> getEtatDentaire() {
        return this.etatDentaire;
    }

    public List<EtatDentaire> controleDentaire(String priorite, String budget) {
        EtatDentaire etatDentaire = new EtatDentaire(this.date, this.id);
        List<EtatDentaire> finis = etatDentaire.controleDentaire(int.Parse(priorite), double.Parse(budget));
        this.aPayer = etatDentaire.prix;
        this.reste = etatDentaire.reste;
        return finis;
    }
    
}
