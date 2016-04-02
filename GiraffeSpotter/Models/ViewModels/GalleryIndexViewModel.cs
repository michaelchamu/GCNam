using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiraffeSpotter.Models.ViewModels
{
    public class GalleryIndexViewModel
    {

        public byte[] imageThumbs { get; set; }
        public byte[] images { get; set; }

    }

    public class GalleryIndexViewModelList
    {

        public List<GalleryIndexViewModel> viewModelList { get; set; }
        public Observation observation { get; set; }
    }

    /// <summary>
    /// The Contents of this MVC 4 Class Was developed and implimented soley by Donovan Maasz in Mey 2014
    /// Email: donovanmaasz@gmail.com
    /// Email: dmdmaasz6.reg@outlook.com
    /// Email: maaszdonovan@live.com
    /// Tel: +264814239878
    /// </summary>
}