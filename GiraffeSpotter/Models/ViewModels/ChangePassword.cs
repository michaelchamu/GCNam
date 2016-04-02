using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiraffeSpotter.Models.ViewModels
{
    public class ChangePassword
    {
        public string username { get; set; }
    }

    public class NewPassword
    {
        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string ReturnToken { get; set; }
    }
}