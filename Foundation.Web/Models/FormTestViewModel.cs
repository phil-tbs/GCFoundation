using System.ComponentModel.DataAnnotations;
using Foundation.Components.Models;
using Foundation.Web.Resources;

namespace Foundation.Web.Models
{
    public class FormTestViewModel: BaseViewModel
    {
        [Required]
        [Display(Name = "Field_Firstname", ResourceType = typeof(Forms))]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Field_Lastname", ResourceType = typeof(Forms))]
        public string Lastname { get; set; }

        public DateOnly TestDate {  get; set; }
        


    }
}
