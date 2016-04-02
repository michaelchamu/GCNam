using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dapper;
using GiraffeSpotter.Models;
using GiraffeSpotter.Models.ViewModels;
using System.Net.Mail;
using GiraffeSpotter.Filters;

namespace GiraffeSpotter.Controllers
{
   
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public ActionResult UserHome(string usernme)
        {
            ViewUser userDetails = new ViewUser();
            usernme = User.Identity.Name;
            userDetails.profilePicture = new Profile_Pictures();
            userDetails.observationCount = 0;
            userDetails.observationCount = (int)con.Query<int>("SELECT COUNT(*) FROM Observations WHERE Username = @user", new { user = @User.Identity.Name }).Single();
            int userIdNum = con.Query<int>("SELECT UserId from UserProfile where username = @usern", new{usern = @usernme}).Single();
            userDetails.regDate = (DateTime)con.Query<DateTime>("SELECT CreateDate FROM webpages_Membership WHERE UserId = @usid", new { usid = @userIdNum }).FirstOrDefault();
            userDetails.userDetails = new UserProfile();
            userDetails.userDetails.UserName = @User.Identity.Name;
            var role =(string) con.Query<string>("SELECT role From Userprofile WHERE Username = @user1", new { user1 = @usernme}).Single();
            userDetails.userDetails.Role = role;
            return View(userDetails);
        }

        public ActionResult Index()
        {
            if (Roles.GetRolesForUser(@User.Identity.Name).Contains("Game Farmer"))
            {
                List<Game_Reserve> reserves = (List<Game_Reserve>) con.Query<Game_Reserve>("SELECT * FROM Game_Reserve WHERE Username = @user",
                                                        new { user = @User.Identity.Name});

                Session["DropDown"] = reserves;
            }
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(EmailViewModel emaildetails)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("info@giraffespotter.org");
                mail.From = new MailAddress("info@giraffespotter.org");
                mail.Subject = emaildetails.Subject;
                string Body = emaildetails.Body;
                mail.Body = Body + "<br /> FROM: <strong>" + emaildetails.Name + "</strong> <br /> EMAIL: " + emaildetails.Email;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.giraffespotter.org";
                smtp.Port = 8889;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("info@giraffespotter.org", "P@ssw0rd!");// Enter senders User name and password
                smtp.EnableSsl = false;
                smtp.Send(mail);

                ViewBag.EmailMsg = "Your Email has successfully been sent, you will hear from our team at GCF shortly.";
                ViewBag.EmailCode = 1;
            }
            catch (Exception e)
            {
                ViewBag.EmailMsg = "Your Email has failed to send, please try again later." + e.Message;
                ViewBag.EmailCode = 0;
            }
            return View();
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
