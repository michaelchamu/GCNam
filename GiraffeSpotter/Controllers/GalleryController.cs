using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using GiraffeSpotter.Models;
using GiraffeSpotter.Models.ViewModels;
using GiraffeSpotter.Models.Service;

namespace GiraffeSpotter.Controllers
{
    public class GalleryController : Controller
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        //
        // GET: /Gallery/

        public ActionResult GalleryView()
        {
            GalleryIndexViewModelList index = new GalleryIndexViewModelList();
            index.viewModelList = new List<GalleryIndexViewModel>();

            var images = con.Query<Gallery>("SELECT * FROM Galleries");


            foreach (var item in images)
            {
                GalleryIndexViewModel single = new GalleryIndexViewModel();
                single.imageThumbs = ResizeImage.ResizeToBase64(item.ImageBytes, 64, 64);
                single.images = ResizeImage.ResizeToBase64(item.ImageBytes, 640, 425);
                index.viewModelList.Add(single);
            }

            return View(index);
        }

        /// <summary>
        /// The Contents of this MVC 4 Controller Class Was Soley Developed and implemented by Donovan Maasz in Mey 2014
        /// Email: donovanmaasz@gmail.com
        /// Email: dmdmaasz6.reg@outlook.com
        /// Email: maaszdonovan@live.com
        /// Tel: +264814239878
        /// </summary>

    }
}
