using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiraffeSpotter.Models;
using System.Drawing;
using GiraffeSpotter.Models.Service;
using System.Web.Configuration;
using GiraffeSpotter.Models.ViewModels;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace GiraffeSpotter.Controllers
{
    public class ObservationController : Controller
    {
        private GCF_ModelsContainer db = new GCF_ModelsContainer();
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        //
        // GET: /Observation/

        public ActionResult Index()
        {
            return View(db.Observations.ToList());
        }

        //
        // GET: /Observation/Details/5

        public ActionResult Details(int id = 0)
        {
            Observation observation = db.Observations.Find(id);
            if (observation == null)
            {
                return HttpNotFound();
            }
            return View(observation);
        }

        //
        // GET: /Observation/Create

        public ActionResult CreateLandingPage() 
        {
            return View();
        }

        //add observation with no image
        public ActionResult CreateNoPicture() 
        {
            Observation observation = new Observation();
            observation.Date_of_Observation = System.DateTime.Now.Date;
            observation.Date_of_Submission = System.DateTime.Now.Date;

            return View(observation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNoPicture(Observation observation) 
        {
            if (ModelState.IsValid)
            {
                Session["imagelessO"] = observation;
                return RedirectToAction("LocationSelect2");
            }
            return View(observation);
        }

        //add observation Uganda
        public ActionResult CreateUganda()
        {
            Observation observation = new Observation();
            observation.Date_of_Observation = System.DateTime.Now.Date;
            observation.Date_of_Submission = System.DateTime.Now.Date;

            return View(observation);
        }

        public ActionResult Create()
        {
            Observation observation = new Observation();

            observation.Date_of_Observation = System.DateTime.Now.Date;
            observation.Date_of_Submission = System.DateTime.Now.Date;

            return View(observation);
        }

        //
        // POST: /Observation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Observation observation, int? origin)
        {
            if (ModelState.IsValid)
            {
                //db.Observations.Add(observation);
                //db.SaveChanges();
                Session["observation"] = observation;
                Session["uganda"] = origin;
                return PartialView("_Gallery");
            }
            HttpRuntimeSection configSection = new HttpRuntimeSection();
            configSection.MaxRequestLength = 1048576;

            return View(observation);
        }

        [HttpPost]
        public ActionResult Gallery(GiraffeSpotter.Models.ViewModels.GalleryViewModel files)
        {
            if (files != null)
            {
                int origin;
                //db.Observations.Add(observation);
                //db.SaveChanges();
                //Session["Files"] = files.images;

                //----------Declrations-----------------//
                RenderImage render = new RenderImage();
                ObservationViewModel model = new ObservationViewModel();
                model.img_bytes = new List<byte[]>();
                MemoryStream fileStream = new MemoryStream();
                //----------Declrations-----------------//

                files.images[0].InputStream.CopyTo(fileStream);

                //----------Image Conversions----------//

                foreach (var item in files.images)
                {
                    item.InputStream.Position = 0;
                    model.img_bytes.Add(render.ConvertToBytes(item, item.InputStream));/*Calls Helper Class RenderImage for HttpPostedFileBase*/
                    item.InputStream.Position = 0;
                }



                Session["bytes"] = model.img_bytes;
                //----------Image Conversions----------//

                //--------Image GPS Extraction---------//
                decimal? lat = EXIF_data.GetLatitude(Image.FromStream(fileStream, true, true));
                decimal? long1 = EXIF_data.GetLongitude(Image.FromStream(fileStream, true, true));

                Session["lat"] = lat;
                Session["long"] = long1;
                if (Session["uganda"] == null)
                {
                    origin = 0;
                }
                else
                {
                    origin = (int)Session["uganda"];
                }

                if (lat == null || long1 == null)
                {
                    if (origin == 0)
                    {
                        return RedirectToAction("LocationSelect");
                    }
                    else
                        if(origin == 1)
                        {
                            return RedirectToAction("Uganda");
                        }
                }
                //--------Image GPS Extraction---------//


                return RedirectToAction("Final", model);
            }


            return View(files);
        }



        public ActionResult LocationSelect()
        {
            return View();
        }

        //Select location for two parks in Uganda
        public ActionResult Uganda() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Uganda(LocationViewModel model)
        {
            Session["lat"] = model.lat;
            Session["long"] = model.lng;
            return RedirectToAction("Final");
        }
        //select location for observation with no image
        public ActionResult LocationSelect2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LocationSelect2(LocationViewModel model)
        {
            Session["lat1"] = model.lat;
            Session["long2"] = model.lng;
            return RedirectToAction("Final2");
        }

        [HttpPost]
        public ActionResult LocationSelect(LocationViewModel model)
        {
            Session["lat"] = model.lat;
            Session["long"] = model.lng;
            return RedirectToAction("Final");
        }

        public ActionResult Final()
        {
            ObservationViewModel model = new ObservationViewModel();
            model.imageList = new List<Models.ViewModels.ObservationGallery>();
            model.img_bytes = (List<byte[]>)Session["bytes"];
            //----------Session Handling------------//
            model.observation = new Observation();
            model.observation = (Observation) Session["observation"];

            //model.images = (HttpPostedFileBase[]) Session["Files"];

            model.Latitude = (decimal) Session["lat"];
            model.Longitude = (decimal) Session["long"];

            Session.Remove("observation");
            Session.Remove("Files");
            Session.Remove("lat");
            Session.Remove("long");

            //----------Session Handling------------//
            
                foreach (var item in model.img_bytes)
                {
                    GiraffeSpotter.Models.ViewModels.ObservationGallery image = new Models.ViewModels.ObservationGallery();
                    image.thumb = ResizeImage.ResizeToBase64(item, 64, 64);
                    image.image = ResizeImage.ResizeToBase64(item, 640, 425);
                    model.imageList.Add(image);
                }
           
            
            return View(model);
        }

        //Final page for observations with no images

        public ActionResult Final2() 
        {
            ImagelessObservation model = new ImagelessObservation();
            model.observation = (Observation)Session["imagelessO"];

            model.Latitude = (decimal)Session["lat1"];
            model.Longitude = (decimal)Session["long2"];
            return View(model);
        }

        [HttpPost]
        public ActionResult Final2(ImagelessObservation model) 
        {
            Locations loc = new Locations();
            loc.Latitude = Math.Round(model.Latitude, 8);
            loc.Longitude = Math.Round(model.Longitude, 8);

            model.observation.Location = new Locations();
            model.observation.Location = loc;
            if (@User.Identity.IsAuthenticated)
            {
                model.observation.Username = @User.Identity.Name;
            }
            else
            {
                model.observation.Username = "anonymous";
            }
            model.observation.Status = "Pending";
            db.Observations.Add(model.observation);
            db.SaveChanges();

            return RedirectToAction("GratitudePage");
        }

        [HttpPost]
        public ActionResult Final(ObservationViewModel model)
        {
            //----------Declrations-----------------//
            RenderImage render = new RenderImage();
            model.img_bytes = (List<byte[]>) Session["bytes"];
            Session.Remove("bytes");
            //----------Declrations-----------------//

            //----------Database Handling------------//
            Locations loc = new Locations();
            loc.Latitude = Math.Round(model.Latitude, 8);
            loc.Longitude = Math.Round(model.Longitude, 8);
            //db.Locations.Add(loc);
            //db.SaveChanges();

            model.observation.Location = new Locations();
            model.observation.Location = loc;
            //if user is logged in and registered, use Identity to get name
            if (@User.Identity.IsAuthenticated)
            {
                model.observation.Username = @User.Identity.Name;
            }
            else
            {
                model.observation.Username = "anonymous";
            }
            model.observation.Status = "Pending";
            db.Observations.Add(model.observation);
            db.SaveChanges();

            render.UploadToGallery(model.img_bytes, model.observation.Id);
            //----------Database Handling------------//

            return RedirectToAction("GratitudePage");
        }

        public ActionResult GratitudePage() 
        {
            return View();
        }

        public JsonResult GetJsonLocations()
        {
            //var test = db.Observations.ToList();
            var observations = con.Query<Observation>("SELECT * FROM Observations");

            foreach (var item in observations)
            {
                int locationId = con.Query<int>("SELECT Location_Id FROM Observations WHERE Id = @observId", new { observId = item.Id }).SingleOrDefault();

                item.Location = new Locations();
                item.Location = con.Query<Locations>("SELECT * FROM Locations WHERE Id = @locId", new { locId = locationId }).SingleOrDefault();
            }

            List<Locations> locations = (List<Locations>) con.Query<Locations>("SELECT * FROM Locations");

            return Json(observations, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ObservationHome()
        {
            return View();
        }

        public ActionResult ViewObservation(int id)
        {
            ObservationViewModel observation = new ObservationViewModel();
            observation.observation = new Observation();
            observation.imageList = new List<Models.ViewModels.ObservationGallery>();

            observation.observation = con.Query<Observation>("SELECT * FROM Observations WHERE Id = @obserId", new { obserId = id }).SingleOrDefault();
            var list = con.Query<byte[]>("SELECT ImageBytes FROM Galleries WHERE ObservationId = @obserId", new { obserId = id });

            foreach (var item in list)
            {
                GiraffeSpotter.Models.ViewModels.ObservationGallery image = new Models.ViewModels.ObservationGallery();
                image.thumb = ResizeImage.ResizeToBase64(item, 64, 64);
                image.image = ResizeImage.ResizeToBase64(item, 640, 425);
                observation.imageList.Add(image);
            }

            observation.Latitude = con.Query<decimal>("SELECT Latitude FROM Locations WHERE Id = @locId", new { locId = observation.observation.Id }).SingleOrDefault();
            observation.Longitude = con.Query<decimal>("SELECT Longitude FROM Locations WHERE Id = @locId", new { locId = observation.observation.Id }).SingleOrDefault();

            return View(observation);
        }

        //
        // GET: /Observation/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Observation observation = db.Observations.Find(id);
            if (observation == null)
            {
                return HttpNotFound();
            }
            return View(observation);
        }

        //
        // POST: /Observation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Observation observation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(observation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(observation);
        }

        //
        // GET: /Observation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Observation observation = db.Observations.Find(id);
            if (observation == null)
            {
                return HttpNotFound();
            }
            return View(observation);
        }

        //
        // POST: /Observation/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Observation observation = db.Observations.Find(id);
            db.Observations.Remove(observation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
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