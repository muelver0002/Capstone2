using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpDevelopMVC4.Models;
namespace SharpDevelopMVC4.Controllers
{
	/// <summary>
	/// Description of ServicesController.
	/// </summary>
	public class ServicesController : Controller
	{
		SdMvc4DbContext _db = new SdMvc4DbContext();
		
		public ActionResult Index()
		{
			if(Session["user"] != null)
			{
			
			var username = Session["user"].ToString();
			var adds = _db.Vetowners.Where(x => x.Username == username).FirstOrDefault();		
			
			int user = adds.Id;
			
			List<Servicesacon> service = _db.Servicesacons.Where(x => x.VetId == user).ToList();
			
			return View(service);   
			}
			
			return RedirectToAction("Logoff", "Account");
		}
		
		
		
		public ActionResult Create()
		{
		
			return View();
		
		}
		[HttpPost]
		public ActionResult Create(Servicesacon service)
		{
		
			
			if(Session["user"] != null)
			{
				var user = Session["user"].ToString();
				var VetId = _db.Vetowners.Where(x => x.Username == user).FirstOrDefault();
				
				int Id = VetId.Id;
				
				service.VetId = Id;
				
			  _db.Servicesacons.Add(service);
			  _db.SaveChanges();
			  return View();
			
			
			}
			return RedirectToAction("Logoff", "Account");
		
		}
		
	}
}