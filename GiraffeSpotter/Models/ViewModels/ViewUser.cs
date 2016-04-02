using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiraffeSpotter.Models.ViewModels
{
    public class ViewUser
    {
        public Profile_Pictures profilePicture { get; set; }
        public GalleryViewModel postedPictures { get; set; }
        public UserProfile userDetails { get; set; }
        public Observation observation { get; set; }
        public int observationCount { get; set; }
        public DateTime regDate { get; set; }
    }
}