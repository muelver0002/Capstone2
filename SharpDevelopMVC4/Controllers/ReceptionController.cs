using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpDevelopMVC4.Models;

namespace SharpDevelopMVC4.Controllers
{

	public class ReceptionController : Controller
	{
		
		SdMvc4DbContext _db = new SdMvc4DbContext();
		public ActionResult Index()
		{
			if(Session["user"] != null)
			{
				var user = Session["user"].ToString();
				var reception = _db.Doctors.Where(x => x.Username == user).FirstOrDefault();
			    int UserId = reception.Vetid;
				List<Patient> patient = _db.Patients.Where(x => x.Vetid == UserId).ToList();
				
				return View(patient);		
			}
			
				return RedirectToAction("Logoff", "Account");
		}
		
		
		
		public ActionResult Add()
		{
			
			return View();
		}
		
		[HttpPost]
		public ActionResult Add(Patient patient)
		{
			
			if(Session["user"] != null)
			{
		     var user = Session["user"].ToString();
		     
		     var Vet = _db.Receptionists.Where(x => x.Username == user).FirstOrDefault();
		     
		     int Id = Vet.VetId;
		     
		     patient.Vetid = Id;
		     
		    _db.Patients.Add(patient);
			_db.SaveChanges();
			return View();
		     
			}
			
		    return RedirectToAction("Logoff", "Account");
		}
		
		
	}
}