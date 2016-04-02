using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using GiraffeSpotter.Models;
using System.Data;
using GiraffeSpotter.Models.ViewModels;
using System.Data.Entity;

namespace GiraffeSpotter.Controllers
{
    
    public class AdminController : Controller
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private UsersContext db = new UsersContext();
        private GCF_ModelsContainer db2 = new GCF_ModelsContainer();
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            if (User.IsInRole("Admin") == true)
            {
                AdminView usercount = new AdminView();
                usercount.newAccounts = (int)con.Query<int>("SELECT COUNT(*) from webpages_Membership WHERE CAST(CreateDate AS DATE)= CAST(GETDATE() AS DATE)").Single();
                usercount.totalUsers = (int)con.Query<int>("SELECT COUNT(*) FROM UserProfile").Single();
                return View(usercount);
            }
            else
                return HttpNotFound();
            
        }

        //returns list of all observations in database
        public ActionResult ManageObservations() 
        {
            if (User.IsInRole("Admin") == true)
            {
                var observations = (List<Observation>)con.Query<Observation>("SELECT * FROM Observations");
                return View(observations);
            }
            else return HttpNotFound();
           
        }

        public ActionResult Database()
        {
            if (User.IsInRole("Admin") == true) 
            {
                return View(); 
            }
            else return HttpNotFound();
        }

        //returns the list of all users in the database
        public ActionResult AllUsers()
        {
            if (User.IsInRole("Admin") == true) 
            {
                var users = (List<UserProfile>)con.Query<UserProfile>("SELECT * FROM UserProfile");
                return View(users);
            }
            return HttpNotFound();
           
        }

        //allows the selection of a single user to view his details in full
        public ActionResult UserDetails(int id = 0)
        {
            if (User.IsInRole("Admin") == true)
            {
                UserProfile userprofile = (UserProfile)con.Query<UserProfile>("SELECT * FROM UserProfile WHERE id = @ID", new { ID = id }).Single();

                if (userprofile == null)
                {
                    return HttpNotFound();
                }
                return View(userprofile);
            }
            else return HttpNotFound();
            
        }

        //deletes a specific user and his whole profile

        // GET: /Profile/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (User.IsInRole("Admin") == true)
            {
                UserProfile userprofile = db.UserProfiles.Find(id);
                if (userprofile == null)
                {
                    return HttpNotFound();
                }
                return View(userprofile);
            }
            else return HttpNotFound();
            
        }

        //
        // POST: /Profile/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userprofile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //allows a selected user profile to be edited by the administrator

        public ActionResult Edit(int id = 0)
        {
            if (User.IsInRole("Admin") == true) 
            {
                UserProfile userprofile = db.UserProfiles.Find(id);
                if (userprofile == null)
                {
                    return HttpNotFound();
                }
                return View(userprofile);
            }
            else
                return HttpNotFound();
        }

        //
        // POST: /Profile/Edit/5

        [HttpPost]
        public ActionResult Edit(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AllUsers");
            }
            return View(userprofile);
        }

        //allows the observation to be edited
        public ActionResult EditObservation(int id = 0)
        {
            if (User.IsInRole("Admin") == true) {
                db.Database.Connection.Close();
                Observation observation = db2.Observations.Find(id);
                if (observation == null)
                {
                    return HttpNotFound();
                }
                return View(observation);
            }
            else return HttpNotFound();
            
        }

        //
        // POST: /Profile/Edit/5

        [HttpPost]
        public ActionResult EditObservation(Observation observation)
        {
            if (ModelState.IsValid)
            {
                db2.Entry(observation).State = EntityState.Modified;
                db2.SaveChanges();
                return RedirectToAction("ManageObservations");
            }
            return View(observation);
        }

        //delete observation
        public ActionResult DeleteObservation(int id = 0)
        {
            if (User.IsInRole("Admin") == true)
            {
                Observation observation = db2.Observations.Find(id);
                if (observation == null)
                {
                    return HttpNotFound();
                }
                return View(observation);
            }
            else return HttpNotFound();

        }

        //
        // POST: /Profile/Delete/5

        [HttpPost, ActionName("DeleteObservation")]
        public ActionResult DeleteConfirmed2(int id)
        {
            Observation observation = db2.Observations.Find(id);
            db2.Observations.Remove(observation);
            db2.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
