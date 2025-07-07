using System.ComponentModel.DataAnnotations;

namespace Honeywell_Production_Dashboard.Models
{
    public class loginmodel
    {
        [Required]
        public string employeeid { get; set; }

        [Required]
        public string password { get; set; }
    }
}
