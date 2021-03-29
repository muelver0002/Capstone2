using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpDevelopMVC4.Models;

namespace SharpDevelopMVC4.Controllers
{
	
	public class DiagnoseController : Controller
	{
		SdMvc4DbContext _db = new SdMvc4DbContext();
		public ActionResult Index()
		{
			if(Session["user"] != null)
			{
				if(User.IsInRole("owner"))
				{
					var user1 = Session["user"].ToString();
					var owner = _db.Vetowners.Where(x => x.Username == user1).FirstOrDefault();
					
					int OwnerId = owner.Id;
					
					List<Diagnose> ownerdiagnose = _db.Diagnoses.Where(x => x.Vetid == OwnerId).OrderByDescending(o => o.Id).ToList();
				
					return View(ownerdiagnose);
							
				}
					
					
				var user = Session["user"].ToString();
				var DocUser = _db.Doctors.Where(x => x.Username == user).FirstOrDefault();
				
				int VetId = DocUser.Vetid;
				
				List<Diagnose> Docdiagnose = _db.Diagnoses.Where(x => x.Vetid == VetId).OrderByDescending(o => o.Id).ToList();
				
				return View(Docdiagnose);
			
			}
			
			
			return RedirectToAction("Logoff", "Account");
		}
		
		
		
		public ActionResult Add(int? Id)
		{
			
		if(Session["user"] !=null)
		  {
			
			var user = Session["user"].ToString();
			var vetid = _db.Doctors.Where(x => x.Username == user).FirstOrDefault();
			int vetId = vetid.Vetid;
		    

            
  			
			Patient patient = _db.Patients.Find(Id);
			if(patient != null)
			{
			int patientId = patient.CustId;
			
			     
			// history per patient  //	
			    ViewBag.history = "";
				if (patientId != 0) {
			
					List<Diagnoserecord> diagnose = _db.Diagnoserecords.Where(x => x.CustId == patientId && x.Vetid == vetId).ToList();
					ViewBag.history = diagnose;
				}
			    
			
			return View(patient);
			}
			return RedirectToAction("index","Diagnose");
			}
		  return RedirectToAction("Logoff", "Account");
		}
		
		[HttpPost]
		public ActionResult Add(Diagnose diagnose,Diagnoserecord history , int ID)
		{
			
			
			if(Session["user"] != null)
			{
				var user = Session["user"].ToString();
				var doctor = _db.Doctors.Where(x => x.Username == user).FirstOrDefault();
				
				int DocId = doctor.Vetid;
				string Docname = doctor.Fullname;
				
				
				diagnose.DocName = Docname;
				diagnose.Vetid = DocId;
				diagnose.Datetoday = DateTime.Now;
				
				TempData["diagnosemsg"]="text";
				TempData["msg"] = "hey";
				
				_db.Diagnoses.Add(diagnose);
				_db.SaveChanges();
			
				//Diagnose History//
				history.DocName = Docname;
				history.Vetid = DocId;
				history.Datetoday = DateTime.Now;
				
				_db.Diagnoserecords.Add(history);
				_db.SaveChanges();
				
				
				var patient = _db.Patients.Find(ID);
				
				if(patient != null)
				{
					_db.Patients.Remove(patient);
					_db.SaveChanges();
					
				}
				
				
				
			   return RedirectToAction("index", "Diagnose");
			
			}
			return RedirectToAction("Logoff", "Account");
		}
	}
}