using GiraffeSpotter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Dapper;
using System.Net.Mail;
using GiraffeSpotter.Models;

namespace GiraffeSpotter.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //
        // GET: /ForgotPassword/

        public ActionResult ForgotP()
        {
            return View();
        }

        public ActionResult returnPartial()
        {
            return PartialView("Step2");
        }

        [HttpPost]
        public ActionResult ForgotP(ChangePassword model)
        {
            try
            {
                var token = WebSecurity.GeneratePasswordResetToken(model.username);
                var person = con.Query<UserProfile>("SELECT * FROM UserProfile WHERE UserName = @user", new { user = model.username }).SingleOrDefault();

                string email = person.Email;

                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("postmaster@giraffespotter.org");
                mail.Subject = "Password Reset Token";
                string Body = "Good Day Sir./Madam. <br /> Please find the reset token below and paste it into the  'code' field on the log in screen. <br /> <strong>" + token + "</strong>";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.giraffespotter.org";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("postmaster@giraffespotter.org ", "Justef@giraffes09");// Enter senders User name and password
                smtp.EnableSsl = false;
                smtp.Send(mail);

                ViewBag.Code = 1;
                ViewBag.Message = "Your Email Has Been sent To your Registered Email Address";
            }
            catch(Exception e)
            {
                ViewBag.Code = 0;
                ViewBag.Message = e.ToString();//"Verification Failed. Please Try Again Later";
            }

            return View();
        }

        [HttpPost]
        public ActionResult ChangeP(NewPassword model)
        {
            bool resetResponse = WebSecurity.ResetPassword(model.ReturnToken, model.Password);
            if (resetResponse)
            {
                ViewBag.Code = 2;
                ViewBag.Message = "Successfully Changed Your Password";
            }
            else
            {
                ViewBag.Code = 3;
                ViewBag.Message = "Failed to Change Your Password!";
            }
            return View("ForgotP");
        }

    }
}
