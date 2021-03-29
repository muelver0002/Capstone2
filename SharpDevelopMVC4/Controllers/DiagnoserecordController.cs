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
	/// Description of DiagnoserecordController.
	/// </summary>
	public class DiagnoserecordController : Controller
	{   
		SdMvc4DbContext _db = new SdMvc4DbContext();
		public ActionResult Index()
		{
			if(Session["user"] != null)
			{
				if(User.IsInRole("owner"))
				{
					var users = Session["user"].ToString();
					var owneruser = _db.Vetowners.Where(x => x.Username == users).FirstOrDefault();
					
					int VetId = owneruser.Id;
					
					List<Diagnoserecord> history = _db.Diagnoserecords.Where(x => x.Vetid == VetId).OrderByDescending(o => o.Id).ToList();				
					return View(history);
				}
				if(User.IsInRole("customer"))
				{
					var cususer = Session["user"].ToString();
					var CustomId = _db.Customers.Where(x => x.Username == cususer).FirstOrDefault();
					
					int customerId = CustomId.Id;
				
					List<Diagnoserecord> custhistory = _db.Diagnoserecords.Where(x => x.CustId == customerId).OrderByDescending(o => o.Id).ToList();
					return View(custhistory);
				}
			        var user = Session["user"].ToString();
					var Docuser = _db.Doctors.Where(x => x.Username == user).FirstOrDefault();
					
					int VetIddoc = Docuser.Vetid;
					List<Diagnoserecord> historydoc = _db.Diagnoserecords.Where(x => x.Vetid == VetIddoc).OrderByDescending(o => o.Id).ToList();				
					return View(historydoc);
			}
			
			
			return RedirectToAction("Logoff", "Account");
		}
		
		
		public ActionResult Add()
		{
			return View();
		}
		
		[HttpPost]
		public ActionResult Add(Diagnoserecord history)
		{
			return View();
		}
		
	}
}