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
	/// Description of ReceptController.
	/// </summary>
	public class ReceptController : Controller
	{
		SdMvc4DbContext _db = new SdMvc4DbContext();
		public ActionResult Index(string Searchkey)
		{   
			if(string.IsNullOrEmpty(Searchkey))
			{
			int num = 0;
			int total = 0;
			foreach(var Item in _db.Recepts.ToList())
			{
				
				num += Item.Total;
				
			
			}
			total = num;
			
			ViewBag.TotalCount = total;
			
			List<Recept> recepts = _db.Recepts.ToList();
			return View(recepts);
			}
			
			else
			{
			  
			int num = 0;
			int total = 0;
			foreach(var Item in _db.Recepts.Where(x => x.CustName.ToLower().Contains(Searchkey.ToLower())).ToList())
			{				
				num += Item.Total;		
			}
			total = num;		
			ViewBag.TotalCount = total;				
			List<Recept> rep = _db.Recepts.Where(x => x.CustName.ToLower().Contains(Searchkey.ToLower())).ToList();
			return View(rep);
				
			}
		}
		
		
		[HttpGet]
		public ActionResult AddItem(int? Id= null)
		{
			if(Session["user"] != null)
			{
				var user = Session["user"].ToString();
				var Docinfo = _db.Doctors.Where(x => x.Username == user).FirstOrDefault();
				int DocId = Docinfo.Vetid;
				
				//ServiceDropdown
				List<Servicesacon> servicelist = _db.Servicesacons.Where(x => x.VetId == DocId).ToList();
				ViewBag.Servicelist = servicelist;
				
				//ServiceDropdown
				List<Medicine> medicines = _db.Medicines.Where(x => x.VetId == DocId).ToList();
				ViewBag.MedList = medicines;
			
			
				List<Productacom> product = _db.Productacoms.Where(x => x.VetId == DocId).ToList();
				ViewBag.prod = product;
				
				List<Recept> recept = _db.Recepts.ToList();
				ViewBag.recpt = recept;
			}
			
				Diagnose diagnose = _db.Diagnoses.Find(Id);
				ViewBag.DiagnoseId = diagnose;
			
			
			return View();
		}
		
		
		[HttpPost]
		public ActionResult AddItem( Recept recepts, int medicineId, int qty1, int serviceId, int qty2, int Id, string cusname)
		{
			var medicines = _db.Medicines.Find(medicineId);
			
			if(medicines != null)
			{
				int medprice = medicines.Price;
				int medtotal =0;
				
				medtotal = medprice * qty1;
				
				recepts.PName = medicines.Name;
				
				recepts.CustName = cusname;
				recepts.CusId = Id;
				recepts.Price = medprice;
				recepts.Total = medtotal;
				recepts.Qty = qty1;
				
				
				
				_db.Recepts.Add(recepts);
				_db.SaveChanges();
			}
			
			var services = _db.Servicesacons.Find(serviceId);
			if(services != null)
			{
				int serviceprice = services.Price;
				int servicetotal = 0;
				
				servicetotal = serviceprice * qty2;
				
				recepts.PName = services.Servicesname;
				recepts.CustName = cusname;
				
				recepts.CusId = Id;
				
				recepts.Price = serviceprice;
				recepts.Total = servicetotal;
				recepts.Qty = qty2;
			    
				_db.Recepts.Add(recepts);
				_db.SaveChanges();
			}
			
			//return views
			List<Recept> recept = _db.Recepts.ToList();
			ViewBag.recpt = recept;
			if(Session["user"] !=null)
			{
		     var user = Session["user"].ToString();
			 var docId = _db.Doctors.Where(x => x.Username ==  user).FirstOrDefault();
				
			 int DocId = docId.Vetid;
				
			List<Medicine> medicine = _db.Medicines.Where(x => x.VetId == DocId).ToList();
			ViewBag.MedList = medicine;
				
		    List<Servicesacon> servicelist = _db.Servicesacons.Where(x => x.VetId == DocId).ToList();
			ViewBag.Servicelist = servicelist;
			}
			Diagnose diagnose = _db.Diagnoses.Find(Id);
			ViewBag.DiagnoseId = diagnose;
			
			return View();
		}
	}
}