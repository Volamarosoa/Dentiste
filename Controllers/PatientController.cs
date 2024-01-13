using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dentiste.Models;

namespace dentiste.Controllers;

public class PatientController : Controller
{

    public IActionResult Index()
    {
        return View("index");
    }

    public IActionResult ajout()
    {
        String nom = Request.Form["nom"].ToString();
        String prenom = Request.Form["prenom"].ToString();
        String date_de_naissance = Request.Form["date_de_naissance"].ToString();
        String date = Request.Form["date"].ToString();
        String idGenre = Request.Form["idGenre"].ToString();
        Patient patient = new Patient(nom, prenom, date_de_naissance, int.Parse(idGenre), date);
        patient.setIdPatient();
        patient.insert();
        return RedirectToAction("etatDentaire", new { idPatient = patient.id, date = date });    
    }

    public IActionResult controle() {
        string Erreur = TempData["Erreur"] as string;
        if (!string.IsNullOrEmpty(Erreur))
        {
            ViewBag.Erreur = Erreur;
        }
        return View("controle");
    }

    // [Route("etatDentaire")]
    public IActionResult etatDentaire(String date, String idPatient) {
        Patient patient = new Patient(idPatient, date);
        patient = patient.getPatient();
        if(patient == null) {
            TempData["Erreur"] = "L'ID patient: " + idPatient + " n'existe pas";
            return RedirectToAction("controle");
        }
        var model = new Dictionary<string, object>();
        model["patient"] = patient;
        model["date"] = date;
        return View("etat_dentaire", model);
        return Ok();
    }

    public ActionResult modifierEtatDentaire() {
        String idPatient = Request.Form["idPatient"].ToString();
        String date = Request.Form["date"].ToString();
        Patient patient = new Patient(idPatient, date);
        patient = patient.getPatient();
        List<EtatDentaire> etats = patient.etatDentaire;
        bool IsTrue = etats[0].IsTrue;
        Console.WriteLine("Modifier");
        for(int i = 0; i < etats.Count(); i++) {
            String etat = Request.Form[""+etats[i].idDent].ToString();
            int valeurEtat = int.Parse(etat);
            if(IsTrue == false) {
                etats[i].etat = valeurEtat;
                etats[i].insert();
            } else if(IsTrue && etats[i].etat != valeurEtat) {
                etats[i].etat = valeurEtat;
                etats[i].updateLastEtat();
                etats[i].insert();
            }
        }
        return RedirectToAction("etatDentaire", new { idPatient = idPatient, date = date });    
    }

    public IActionResult controleDentaire() {
        String idPatient = Request.Form["idPatient"].ToString();
        String date = Request.Form["date"].ToString();
        String priorite = Request.Form["priorite"].ToString();
        String budget = Request.Form["budget"].ToString();
        Console.WriteLine(idPatient + " " + date + " " + priorite + " " + budget);
        Patient patient = new Patient(idPatient, date);
        List<EtatDentaire> finis = patient.controleDentaire(priorite, budget);
        Console.WriteLine(finis.Count());
        var model = new Dictionary<string, object>();
        model["finis"] = finis;
        model["payer"] = patient.aPayer;
        model["reste"] = patient.reste;
        model["date"] = date;
        model["idPatient"] = idPatient;
        return View("finis", model);
    }

}
