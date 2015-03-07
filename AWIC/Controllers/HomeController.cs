using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AWIC.Helpers;

namespace AWIC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult SendTestEmail(string emailid)
        {
            if(User.Identity.IsAuthenticated)
            {
                AWICEmailHelper emailHelper = new AWICEmailHelper();

                try
                {
                    emailHelper.SendTestEmail(emailid);

                    TempData["AlertMessage"] = "Email sent. Check email. ";
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    TempData["AlertMessage"] = "Exception: " + ex.ToString();
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            // Pass list of files in a string array to the view
            string[] allfiles = Directory.GetFiles(Server.MapPath("~/Content/gallery"));
            
            if (allfiles.Length > 6)
            {
                List<string> files = new List<string>();
                for (int i = 0; i < 6; i++)
                {
                    files.Add(allfiles[i]);
                }
                ViewBag.Files = files;
            }
            else
                ViewBag.Files = allfiles;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        
        public ActionResult Settlement()
        {
            return View();
        }

        public ActionResult Community()
        { 
            return View();
        }

        public ActionResult Seniors()
        {
            return View();
        }

        public ActionResult Other()
        {
            return View();
        }

        public ActionResult Funders()
        {
            return View();
        }
        public ActionResult Sponsors()
        {
            return View();
        }

        public ActionResult Careers()
        {
            return View();
        }
    }
}