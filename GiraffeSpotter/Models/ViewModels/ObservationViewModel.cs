using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiraffeSpotter.Models.ViewModels
{
    public class ObservationViewModel
    {
        /// <summary>
        /// Observation Details
        /// </summary>
        public Observation observation { get; set; }

        /// <summary>
        /// Location Details
        /// </summary>
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        /// <summary>
        /// Gallery Information
        /// </summary>
        public HttpPostedFileBase[] images { get; set; }

        public List<ObservationGallery> imageList { get; set; }

        public List<byte[]> img_bytes { get; set; }
    }

    public class ObservationGallery
    {
        public byte[] thumb { get; set; }
        public byte[] image { get; set; }
    }

    public class ImagelessObservation
    {
        /// <summary>
        /// Observation Details
        /// </summary>
        public Observation observation { get; set; }

        /// <summary>
        /// Location Details
        /// </summary>
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string LocationDetails { get; set; }

    }

    /// <summary>
    /// The Contents of this MVC 4 Controller Class Was Soley Developed and implemented by Donovan Maasz in Mey 2014
    /// Email: donovanmaasz@gmail.com
    /// Email: dmdmaasz6.reg@outlook.com
    /// Email: maaszdonovan@live.com
    /// Tel: +264814239878
    /// </summary>
}