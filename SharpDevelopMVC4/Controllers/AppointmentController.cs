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
     		if(Session["user"] != null)
			{
			var username = Session["user"].ToString();
			var Vet = _db.Receptionists.Where(x => x.Username == username).FirstOrDefault();		
			int userId = Vet.VetId;
			
			List<Appointment> appointment = _db.Appointments.Where(x => x.VetId == userId).ToList();
			
			return View(appointment);   
			}
			
			return RedirectToAction("Logoff", "Account");
			
			
//			var list = _db.Appointments.ToList();		
//			return View(list);
		}
		
		
		
		public ActionResult Add(int ID)
		{
			
//			var adds = _db.Products.Where(x => x.Id == ID).FirstOrDefault();					
//			int user = adds.Id;			
//			List<Servicesacon> services =_db.Servicesacons.Where(x => x.VetId == user).ToList();
//			ViewBag.service = services;
			if(User.IsInRole("customer"))
            {		
				if(Session["user"] != null)
				{
					 ViewBag.Id = ID;
					
					var username = Session["user"].ToString();
					var cus = _db.Customers.Where(x => x.Username == username).FirstOrDefault();
					
					int cusId = cus.Id;
					
					Customer customer = _db.Customers.Find(cusId);
					
					return View(customer);
					
					
				
				}
			}
			
			return View();
		
		}
		
		
		[HttpPost]
		public ActionResult Add(Appointment app)
		{
		 
			_db.Appointments.Add(app);
			_db.SaveChanges();
			
			return RedirectToAction("index", "crudsample");
		
		}
	}
}