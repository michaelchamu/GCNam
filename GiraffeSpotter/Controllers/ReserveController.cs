using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiraffeSpotter.Models;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using GiraffeSpotter.Models.ViewModels;

namespace GiraffeSpotter.Controllers
{
    public class ReserveController : Controller
    {
        private GCF_ModelsContainer db = new GCF_ModelsContainer();

        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        //
        // GET: /Reserve/

        public ActionResult Index()
        {
            try
            {
                var reserves = con.Query<Game_Reserve>("SELECT * FROM Game_Reserve WHERE Username = @user",
                                                        new { user = @User.Identity.Name });
                return View(reserves);
            }
            catch (Exception e)
            {
                return RedirectToAction("Unhandled_E", "Home");
            }
        }

        //
        // GET: /Reserve/Details/5

        public ActionResult Details(int id = 0)
        {
            ReserveManage model = new ReserveManage();
            model.reserve = db.Game_Reserve.Find(id);

            model.giraffe = (List<Giraffe>) con.Query<Giraffe>("SELECT * FROM Giraffes WHERE Game_ReserveId = @Id",
                                                        new { Id = model.reserve.Id });

            if (model.reserve == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult C_Tanslocate(Int32 reserve_Id)
        {
            Giraffe model = new Giraffe();
            model.Game_ReserveId = reserve_Id;

            return View(model);
        }

        [HttpPost]
        public ActionResult C_Tanslocate(Giraffe model)
        {
            model.Status = "Pending";
            try
            {
                if (ModelState.IsValid)
                {
                    db.Giraffes.Add(model);
                    db.SaveChanges();
                }
                else
                {
                    return View(model);
                }

                return RedirectToAction("Details", new { id = model.Game_ReserveId });
            }
            catch (Exception e)
            {
                return View(model);
            }
        }

        //
        // GET: /Reserve/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Reserve/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Game_Reserve game_reserve)
        {
            game_reserve.Status = "Pending";
            game_reserve.Username = @User.Identity.Name;


            if (ModelState.IsValid)
            {
                //db.Game_Reserve.Add(game_reserve);
                //db.SaveChanges();
                Session["reserve"] = game_reserve;
                return PartialView("_Location");
            }

            return View(game_reserve);
        }

        [HttpPost]
        public ActionResult Location(Locations location)
        {
            Game_Reserve reserve = new Game_Reserve();
            reserve = (Game_Reserve)Session["reserve"];
            reserve.Location = new Locations();
            Session.Remove("reserve");

            db.Locations.Add(location);
            db.SaveChanges();


            reserve.Location.Id = location.Id;
            db.Game_Reserve.Add(reserve);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /Reserve/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Game_Reserve game_reserve = db.Game_Reserve.Find(id);
            if (game_reserve == null)
            {
                return HttpNotFound();
            }
            return View(game_reserve);
        }

        //
        // POST: /Reserve/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Game_Reserve game_reserve)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game_reserve).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(game_reserve);
        }

        //
        // GET: /Reserve/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Game_Reserve game_reserve = db.Game_Reserve.Find(id);
            if (game_reserve == null)
            {
                return HttpNotFound();
            }
            return View(game_reserve);
        }

        //
        // POST: /Reserve/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game_Reserve game_reserve = db.Game_Reserve.Find(id);
            db.Game_Reserve.Remove(game_reserve);
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