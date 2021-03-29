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
	/// Description of MedicineController.
	/// </summary>
	public class MedicineController : Controller
	{   
		SdMvc4DbContext _db = new SdMvc4DbContext();
		public ActionResult Index()
		{
			return View();
		}
		
		
		[HttpGet]
		public ActionResult Addmed()
		{
			return View();
		}
		
		[HttpPost]
		public ActionResult addmed(Medicine medicines)
		{
			if(Session["user"] != null)
			{
				var user = Session["user"].ToString();
				var OwnerId = _db.Vetowners.Where(x => x.Username == user).FirstOrDefault();
				
				int VetsId = OwnerId.Id;
				medicines.VetId = VetsId;
			  
				_db.Medicines.Add(medicines);
				_db.SaveChanges();
			}
			
			
			return View();
		}
	}
}