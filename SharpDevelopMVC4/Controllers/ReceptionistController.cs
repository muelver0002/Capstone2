using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpDevelopMVC4.Models;

namespace SharpDevelopMVC4.Controllers
{
	
	public class ReceptionistController : Controller
	{
		SdMvc4DbContext _db = new SdMvc4DbContext();
		public ActionResult Index()
		{
			if(Session["user"] != null)
			{
				if(User.IsInRole("owner")){
				var user = Session["user"].ToString();
				var vetadmin = _db.Vetowners.Where(x => x.Username == user).FirstOrDefault();
				
				int VetId = vetadmin.Id;
				
				List<Receptionist> receptionlist = _db.Receptionists.Where(x => x.VetId == VetId).ToList();
				
				return View(receptionlist);
			
				}
			}
			
			
			return RedirectToAction("Logoff", "Account");
		}
		
		
		[HttpGet]
		public ActionResult Register()
		{
			return View();
		}
		
		[HttpPost]
		public ActionResult Register(RegisterViewModel newUser, string RetypePassword)
		{
		
			if(Session["user"] != null)
			{
				var user = Session["user"].ToString();
				var vetId = _db.Vetowners.Where(x => x.Username == user).FirstOrDefault();
				int Id = vetId.Id;
			
			if(newUser.Password == RetypePassword)
			{
			
				var Regis = UserAccount.Create(newUser.UserName, newUser.Password, "recept");
				if(Regis != null)
				{
					var newRecept = new Receptionist();
					
					newRecept.Fullname = newUser.Fullname;
					newRecept.Username = newUser.UserName;
					newRecept.Password = newUser.Password;
					newRecept.VetId = Id;
					ViewBag.reg ="text";
					_db.Receptionists.Add(newRecept);
					_db.SaveChanges();
					
					
					return View();
				
				
				}
				
				ViewBag.messages = "Registration Failed";
			
			
			}
			
			else{
				ViewBag.message="Password not matched";
			   }
			   return View();
			}
			
			
			return RedirectToAction("Logoff", "Account");
		}
		
		
		
	}
}