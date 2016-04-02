using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using GiraffeSpotter.Models;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace GiraffeSpotter.Models.Service
{
    public class RenderImage
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public void UploadToDB(HttpPostedFileBase file, string username)
        {
            Profile_Pictures img = new Profile_Pictures();
            img.Extension = file.ContentType;
            img.Username = username;
            img.ImageByte = ConvertToBytes(file, file.InputStream);
            using (GCF_ModelsContainer db = new GCF_ModelsContainer())
            {
                db.Profile_Pictures.Add(img);
                db.SaveChanges();
            }
        }

        public bool pfChange(HttpPostedFileBase file, string username)
        {
            Profile_Pictures img = new Profile_Pictures();
            img.Extension = file.ContentType;
            img.Username = username;
            img.ImageByte = ConvertToBytes(file, file.InputStream);

            int? User = con.Query<int>("SELECT Id FROM Profile_Pictures WHERE username = @User",
                                        new { User = username }).SingleOrDefault();

            if (User == 0) {
                User = null;
            }

            try
            {
                if (User == null)
                {
                    using (GCF_ModelsContainer db = new GCF_ModelsContainer())
                    {
                        db.Profile_Pictures.Add(img);
                        db.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    con.Execute("UPDATE Profile_Pictures SET ImageByte = @bytes, Extension = @ext, Username = @name",
                                new { bytes = img.ImageByte, ext = img.Extension, name = img.Username });
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public byte[] ConvertToBytes(HttpPostedFileBase file, Stream stream)
        {
            try
            {
                byte[] imagebytes = null;
                BinaryReader img_reader = new BinaryReader(stream);
                imagebytes = img_reader.ReadBytes((int)file.ContentLength);
                return imagebytes;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public byte[] GetImageBytes(string user)
        {
            using (con)
            {
                con.Open();
                var prof = con.Query<Profile_Pictures>("SELECT * FROM Profile_Pictures WHERE Username = @Username",
                                                        new { Username = user }).SingleOrDefault();
                con.Close();
                if (prof == null)
                {
                    return null;
                }
                else
                {
                    return prof.ImageByte;
                }
            }
        }

        public void UploadToGallery(List<byte[]> images, int observation_id)
        {
            Gallery img = new Gallery();
            img.ObservationId = observation_id;


            //img.ImageBytes = ConvertToBytes(file, file.InputStream);
            foreach (var image in images)
            {
                img.ImageBytes = image;
                img.Caption = System.DateTime.Now.Date.ToString();

                using (GCF_ModelsContainer db = new GCF_ModelsContainer())
                {
                    db.Galleries.Add(img);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// The Contents of this MVC 4 Class Was developed and implimented soley by Donovan Maasz in Mey 2014
        /// Email: donovanmaasz@gmail.com
        /// Email: dmdmaasz6.reg@outlook.com
        /// Email: maaszdonovan@live.com
        /// Tel: +264814239878
        /// </summary>

    }
}