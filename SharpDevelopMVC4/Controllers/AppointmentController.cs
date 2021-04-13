using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpDevelopMVC4.Models;

namespace SharpDevelopMVC4.Controllers
{
	
	public class AppointmentController : Controller
	{
		SdMvc4DbContext _db = new SdMvc4DbContext();
		
		public ActionResult Index()
		{
			string status = "Pending";
     		if(Session["user"] != null)
			{
     			if(User.IsInRole("owner"))
     			{
     				var user = Session["user"].ToString();
     				var vetuser = _db.Vetowners.Where(x => x.Username == user ).FirstOrDefault();     			   
     				int VetUserId = vetuser.Id;
     				
     				List<Appointment> Adminappointment = _db.Appointments.Where(x => x.VetId == VetUserId && x.Status.ToLower().Contains(status.ToLower())).OrderByDescending(o => o.Id).ToList();
     			    
     			    return View(Adminappointment);
     			
     			}
     			
     			
     			string Status ="Pending";
			var username = Session["user"].ToString();
			var Vet = _db.Receptionists.Where(x => x.Username == username).FirstOrDefault();		
			int userId = Vet.VetId;
			
			List<Appointment> appointment = _db.Appointments.Where(x => x.VetId == userId && x.Status.ToLower().Contains(Status.ToLower())).OrderByDescending(o => o.Id).ToList();
			
			return View(appointment);   
			}
			
			return RedirectToAction("Logoff", "Account");
			
			
//			var list = _db.Appointments.ToList();		
//			return View(list);
		}
		
		
		
		
		public ActionResult Appointmnet()
		{
			if(Session["user"] != null)
			{
					
			if(User.IsInRole("customer"))
     			{
     				ViewBag.Status="Approved";
     				
     				var usercus = Session["user"].ToString();
     				var custId = _db.Customers.Where(x => x.Username == usercus ).FirstOrDefault();     				
     				int CustId = custId.Id;
     			
     				List<Appointment> forcustomer = _db.Appointments.Where(x => x.CustId == CustId).OrderByDescending(o => o.Id).ToList();
     				
     				return View(forcustomer);
     			
     			}
			
			}
			
			return RedirectToAction("Logoff","Account");
		
		
		}
		
		
		public ActionResult Add(int? ID= null)
		{
			

			if(User.IsInRole("customer"))
            {		
				if(Session["user"] != null)
				{
					
					
					Product vetname = _db.Products.Find(ID);
					ViewBag.Name = vetname.Name;
					ViewBag.Id = ID;
					
					var username = Session["user"].ToString();
					var cus = _db.Customers.Where(x => x.Username == username).FirstOrDefault();
					
					int cusId = cus.Id;
									
					List<Servicesacon> Serviceid = _db.Servicesacons.Where(x => x.VetId == ID).ToList();
					ViewBag.services = Serviceid;
					
					
					List<Pet> petId = _db.Pets.Where(x => x.OwnersID == cusId).ToList();
					ViewBag.pets = petId;
										
					Customer customer = _db.Customers.Find(cusId);
					
					return View(customer);
			
				}
			}
			
			return View();
		
		}
		
		
		[HttpPost]
		public ActionResult Add(Appointment app, int Pet, string[] Services)
		{
			int Id = app.VetId;
			string serve="";
			foreach(var service in Services)
			{
			   
				serve += service +", ";
			
			}
			
			
			var Petinfo = _db.Pets.Where(x => x.Id == Pet).FirstOrDefault();
			  
			app.PetName = Petinfo.PetName;
			app.Color = Petinfo.Color;
			app.Type = Petinfo.Type;
			app.Breed = Petinfo.Breed;
			app.Bloodtype = Petinfo.Bloodtype;
			app.Gender = Petinfo.Gender;
			app.Bdate = Petinfo.Bdate;
			app.Status = "Pending";
			app.Concern = serve;
			
			TempData["customer"] = "customer";
			_db.Appointments.Add(app);
			_db.SaveChanges();
			
			Product product = _db.Products.Find(Id);
			
			return RedirectToAction("details","crudsample", product);
		
		}
		
		
		public ActionResult Delete(int Id)
		{
			var app = _db.Appointments.Find(Id);
			if(app != null)
			{
				_db.Appointments.Remove(app);
				_db.SaveChanges();	
				TempData["deletemsg"] ="text";
			}
			return RedirectToAction("Appointmnet");
		
		}
		
		
	}
}