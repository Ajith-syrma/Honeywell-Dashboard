using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Honeywell_Production_Dashboard.Models
{
    public class loginmodel
    {
        [Required]
        public string employeeid { get; set; }

        [Required]
        public string password { get; set; }


        [BindNever]
        [ValidateNever]
        public string usertype { get; set; }  // Make sure this exists and is public


    }
}
