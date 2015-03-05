using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AWIC.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            // Pass list of files in a string array to the view
            List<string> allfiles = Directory.GetFiles(Server.MapPath("~/Content/gallery")).ToList<string>(); ;

            ViewBag.Files = allfiles;

            List<bool> wides = new List<bool>();
            List<string> sizes = new List<string>();
            foreach (string file in allfiles)
            {
                Image img = new Bitmap(file);
                wides.Add(img.Width > img.Height ? true : false);

                string dataSize = img.Width + "x" + img.Height;
                sizes.Add(dataSize);
            }

            ViewBag.Wides = wides;
            ViewBag.Sizes = sizes;

            return View();
        }
    }
}