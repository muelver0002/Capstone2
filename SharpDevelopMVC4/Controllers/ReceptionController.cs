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
				 if(User.IsInRole("owner"))
			     {
				 	var VetUser = Session["user"].ToString();
				 	var AdminVet= _db.Vetowners.Where(x => x.Username == VetUser ).FirstOrDefault();
				 	
				 	int VetId = AdminVet.Id;
				 	List<Patient> addminpatient = _db.Patients.Where(x => x.Vetid == VetId).OrderByDescending(o => o.Id).ToList();
				 	
				 	return View(addminpatient);
				 }
				 if(User.IsInRole("recept"))
				 {
				 	var receptuser = Session["user"].ToString();
				 	var RecepVet =_db.Receptionists.Where(x => x.Username == receptuser).FirstOrDefault();
				 	
				 	int ReceptId = RecepVet.VetId;
				 	
				 	List<Patient> patient1 = _db.Patients.Where(x  => x.Vetid == ReceptId).OrderByDescending(o => o.Id).ToList();
				 		
				 	return View(patient1);
				 }
				
				
				var user = Session["user"].ToString();	
				var reception = _db.Doctors.Where(x => x.Username == user).FirstOrDefault();
			    int UserId = reception.Vetid;
			    List<Patient> patient = _db.Patients.Where(x => x.Vetid == UserId).OrderByDescending(o => o.Id).ToList();
				
				return View(patient);		
			}
			
				return RedirectToAction("Logoff", "Account");
		}
		
		
		//for Doctor
		public ActionResult Add(int Id)
		{     
			if(User.IsInRole("recept"))
			{
				if(Session["user"] != null)
				{
				
			    Appointment appoinments = _db.Appointments.Find(Id);
			    if(appoinments != null)
			    {
				return View(appoinments);
			    }
			   return RedirectToAction("index","Reception");
				}
			
			}
			return View();
				
			  
		}
		
		[HttpPost]
		public ActionResult Add(Patient patient, int ID)
		{
			
			if(Session["user"] != null)
			{
		     var user = Session["user"].ToString();		     
		     var Vet = _db.Receptionists.Where(x => x.Username == user).FirstOrDefault();
		     
		     
		     int Id = Vet.VetId;
		     Product vetname = _db.Products.Find(Id);
		     
		     patient.Datetoday = DateTime.Now;
		     patient.Vetid = Id;
		     
		     patient.Vetname = vetname.Name;
		     
		     ViewBag.message = "confirm";
		     
		    TempData["addmessage"] = "message"; 
		    _db.Patients.Add(patient);
			_db.SaveChanges();
			
			
			var app = _db.Appointments.Find(ID);
			if(app != null)
			{
				TempData["approve"] ="yes";
				_db.Appointments.Remove(app);
				_db.SaveChanges();
			
			}
			
			
			return RedirectToAction("index","Reception");
		     
			}
			

			return View();
		}
		
		
	
		[HttpGet]
		public ActionResult Addpatient()
		{
		  if(Session["user"] != null)
			{
				var user = Session["user"].ToString();
				var vetId = _db.Receptionists.Where(x => x.Username == user).FirstOrDefault();
				
				int ID = vetId.VetId;
				
			    List<Servicesacon> SerrvId = _db.Servicesacons.Where(x => x.VetId == ID).ToList();
					
				ViewBag.Id = SerrvId;
				
				
			    return View();
		    }
		       return RedirectToAction("Logoff", "Account");
		}
		
		[HttpPost]
		public ActionResult Addpatient(Patient patient, string[] Services)
		{
			
			if(Session["user"] != null)
			{
				string serve="";
				foreach(var service in Services)
				{
				   
					serve += service +", ";
				
				}
				
				var user = Session["user"].ToString();
				var Vet = _db.Receptionists.Where(x => x.Username == user).FirstOrDefault();				
				int Receptid = Vet.VetId;
				
				patient.Concern = serve;
			    patient.Datetoday = DateTime.Now;
			    patient.Vetid = Receptid;
			    
			    ViewBag.message = "confirm";
			    TempData["addpatient"] ="text";
			    _db.Patients.Add(patient);
			    _db.SaveChanges();
			    
			    return RedirectToAction("index","Reception");
			  
			
			}
			
		    return RedirectToAction("Logoff", "Account");
		
		}
		
		
	}
}