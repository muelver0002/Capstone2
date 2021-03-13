﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using Dapper;
using Newtonsoft.Json;

namespace SharpDevelopMVC4.Controllers
{
    public class HomeController : Controller
    {    	
        public ActionResult Index()
        {
        	string mdb = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.GetData("DataDirectory") + @"\MyAccessDb.mdb";
        	


            using (var conn = new OleDbConnection(mdb))
            {
                // conn.Execute( "INSERT INTO contacts(FullName, Email, BirthDate) "
                // + "VALUES (@FullName, @Email, @BirthDAte)", contact);
                var contactList = conn.Query("Select Id, FullName, Email, BirthDate from contacts").ToList();
                ViewBag.Data = JsonConvert.SerializeObject(contactList);
            }


            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        [Authorize]
        public ActionResult ForAuthUser()
        {
        	ViewBag.Message = "Authorized user page. ";

            return View("About");
        }
        
        [Authorize(Roles="admin")]
        public ActionResult ForRoleUser()
        {
            ViewBag.Message = "Authorized ADMIN page.";

            return View("About");
        }        
                
    }
}