using Antlr.Runtime;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Prog7311.model
{
    
    public class Farmers 
    {

        [Required]
        public string UserRoles { get; set; }

        [Key] 
        public String FarmerId { get; set; }
        
        //[ForeignKey("UserId")]
        public String UserId { get; set; } // Foreign key property

       // public RegisterViewModel User { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public string ProductType { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
//---------------------------------------------End of File-------------------------------------------------------\\