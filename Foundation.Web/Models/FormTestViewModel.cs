using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using Foundation.Components.Attributes;
using Foundation.Components.Models;
using Foundation.Web.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Foundation.Web.Models
{
    public class FormTestViewModel: BaseViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FullName_Label", ResourceType = typeof(Forms))]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email_Label", ResourceType = typeof(Forms))]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password_Label", ResourceType = typeof(Forms))]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Url, ErrorMessageResourceType = typeof(Forms), ErrorMessageResourceName = "InvalidUrl")]
        [Display(Name = "Website_Label", ResourceType = typeof(Forms))]
        public string Website { get; set; } = string.Empty;

        [Required]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
        [Display(Name = "Age_Label", ResourceType = typeof(Forms))]
        public int Age { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Bio_Label", ResourceType = typeof(Forms))]
        public string Bio { get; set; } = string.Empty;


        [Required]
        [LocalizedFieldMetadata(typeof(Forms), "Country_Label", "Country_hint")]
        public string SelectedCountry { get; set; } = string.Empty;

        public List<SelectListItem> CountryOptions { get; set; } = new()
        {
            new SelectListItem { Value = "CA", Text = "Canada" },
            new SelectListItem { Value = "US", Text = "United States" },
            new SelectListItem { Value = "FR", Text = "France" },
            new SelectListItem { Value = "DE", Text = "Germany" }
        };

        [Required]
        [Display(Name = "Gender_Label", ResourceType = typeof(Forms))]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [Display(Name = "AgreeToTerms_Label", ResourceType = typeof(Forms))]
        public bool AgreeToTerms { get; set; }


    }
}
