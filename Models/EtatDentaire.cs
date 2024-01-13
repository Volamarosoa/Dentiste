using System;
using Npgsql;
using System.Data.SqlClient;

namespace dentiste.Models;

public class EtatDentaire  {
    public int id { get; set; }
    public String date { get; set; }
    public int idDent { get; set; }
    public String idPatient { get; set; }
    public int etat { get; set; }

    public bool IsTrue { get; set; }

    public double prix { get; set; }
    public double reste { get; set; }

    public EtatDentaire() {}

    public EtatDentaire(String date, String idPatient) {
        this.date = date;
        this.idPatient = idPatient;
    }

    public EtatDentaire(String date, String idPatient, String idDent, String Etat) {
        this.date = date;
        this.idPatient = idPatient;
        this.idDent = int.Parse(idDent);
        this.etat = int.Parse(Etat);
    }

    public String getCouleur() {
        if (this.etat == 0)
            return "red";
        else if(this.etat > 0 && this.etat <= 3)
            return "black";
        else if(this.etat >= 4 && this.etat <= 6)
            return "brown";
        else if(this.etat >= 7 && this.etat <= 9)
            return "yellow";
        return "rgb(225, 48, 139)";
        // return "white";
    }

    public String getCouleurString() {
        if (this.etat == 0)
            return "white";
        else if(this.etat > 0 && this.etat <= 3)
            return "white";
        else if(this.etat >= 4 && this.etat <= 6)
            return "white";
        else if(this.etat >= 7 && this.etat <= 9)
            return "black";
        return "black";
    }

    public void SetEtat() {
        if(this.etat > 0)
            etat -= 1;
        etat = 10;
    }

    public void insert() {
        Connexion connexion = new Connexion();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                String commande = "insert into Etat_dentaire(date, idpatient, iddent, etat) values('"+this.date+"', '"+this.idPatient+"', "+this.idDent+", "+this.etat+")";
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

    public void ajouControle() {
        Connexion connexion = new Connexion();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                String commande = "insert into controle_dentaire(idetat_dentaire, prix) values('"+this.id+"', '"+this.prix+"')";
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

    public void updateLastEtat() {
        Connexion connexion = new Connexion();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                String commande = "update Etat_dentaire set isLast = 0 where idPatient = '"+this.idPatient+"' and idDent = "+this.idDent;
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
    
    public List<EtatDentaire> getListeEtatDentaire() {
        Connexion connexion = new Connexion();
        List<EtatDentaire> liste = new List<EtatDentaire>();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = "SELECT * FROM Etat_Dentaire where idPatient = '" + idPatient +"' and isLast = 1 order by idDent asc";
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EtatDentaire etat = new EtatDentaire(reader.GetDateTime(1).ToString("dd/MM/yyyy"),reader.GetString(2), ""+reader.GetInt32(3), ""+reader.GetInt32(4));
                            etat.id = reader.GetInt32(0);
                            liste.Add(etat);
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

    public List<EtatDentaire> getEtatDentaire() {
        List<EtatDentaire> etats = this.getListeEtatDentaire();
        if(etats.Count() == 0) {
            Dent dentaire = new Dent();
            List<Dent> listeDents = dentaire.getListeDent();
            foreach(Dent dent in listeDents) {
                EtatDentaire etat = new EtatDentaire(this.date, this.idPatient, ""+dent.id, "10");
                etat.IsTrue = false;
                etats.Add(etat);
            }
        }else {
            etats[0].IsTrue = true;
        }
        return etats;
    }

    public List<EtatDentaire> getListeEtatParPriorite(int priorite) {
        Connexion connexion = new Connexion();
        List<EtatDentaire> liste = new List<EtatDentaire>();
        try 
        {
            using (NpgsqlConnection conn = connexion.connection) // Utilisez NpgsqlConnection
            {
                conn.Open();
                string commande = "select * from liste_etat_dentaire where idPatient = '"+ this.idPatient +"' and etat != 10 order by reste asc, place asc, type asc";
                if(priorite == 20)
                    commande = "select * from liste_etat_dentaire where idPatient = '"+ this.idPatient +"' and etat != 10 order by reste desc, place asc, type asc";
                Console.WriteLine(commande);
                using (NpgsqlCommand cmd = new NpgsqlCommand(commande, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EtatDentaire etat = new EtatDentaire(reader.GetDateTime(1).ToString("dd/MM/yyyy"),reader.GetString(2), ""+reader.GetInt32(3), ""+reader.GetInt32(4));
                            etat.id = reader.GetInt32(0);
                            liste.Add(etat);
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

    public List<EtatDentaire> controleDentaire(int priorite, double budget) {
        bool isBudget = true;
        if(budget == 0)
            isBudget = false;
        double somme = 0;
        List<EtatDentaire> finis = new List<EtatDentaire>();
        List<EtatDentaire> etats = this.getListeEtatParPriorite(priorite);
        for(int i=0; i<etats.Count; i++) {
            bool stop = false;
            double avant = somme;
            while(etats[i].etat != 10) {
                Type type = new Type();
                type = type.getType(this.date, etats[i].idDent, etats[i].etat);
                int diff = (type.max + 1) - etats[i].etat;
                double prix = diff*type.prix;
                int nextEtat = type.max + 1;
                if(type.max == 3)
                    nextEtat = 0;
                else if(type.max == 0)
                    nextEtat = 10;
                Console.WriteLine("Dent:" + etats[i].idDent + " etat:" + etats[i].etat + " prix:" + prix + " diff:" + diff + " max:" + type.max);
                if(isBudget) {
                    if(budget >= prix) {
                        // finis.Add(etats[i]);
                        etats[i].etat = nextEtat;
                        etats[i].prix = prix;
                        somme += prix;
                        budget -= prix;
                    } else {
                        if(budget == 0)
                            stop = true;
                        break;
                    }
                } else {
                    etats[i].etat = nextEtat;
                    etats[i].prix = prix;
                    somme += prix;
                }
            }
            if(somme > avant) {
                double diff = somme - avant;
                finis.Add(etats[i]);
                etats[i].updateLastEtat();
                etats[i].insert();
                etats[i].ajouControle();
            }
            if(stop)
                break;
        }
        Console.WriteLine("*****************Fini****************** somme: " + somme + " et reste: " + budget);
        for(int i = 0; i <finis.Count(); i++) {
            Console.WriteLine("Dent:" + finis[i].idDent + " etat:" + finis[i].etat);
        }
        this.prix = somme;
        this.reste = budget;
        return finis;
     }

    
}
