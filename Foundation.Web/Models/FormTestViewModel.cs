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
        public int? Age { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "DateOfBirth_Label", Description = "DateOfBirth_Hint", ResourceType = typeof(Forms))]
        [DateFormat("full")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Bio_Label", Description = "Bio_Hint" , ResourceType = typeof(Forms))]
        public string Bio { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Country_Label", Description = "Country_Hint", ResourceType = typeof(Forms))]
        public string? SelectedCountry { get; set;}

        public IEnumerable<SelectListItem> CountryOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "CA", Text = "Canada" },
            new SelectListItem { Value = "US", Text = "United States" },
            new SelectListItem { Value = "FR", Text = "France" },
            new SelectListItem { Value = "DE", Text = "Germany" }
        };

        [Required]
        [Display(Name = "Gender_Label", ResourceType = typeof(Forms))]
        public string Gender { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> GenderOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Male", Text = "Male" },
            new SelectListItem { Value = "Female", Text = "Female" },
            new SelectListItem { Value = "Other", Text = "Other" }
        };

        [Required]
        [Display(Name = "AgreeToTerms_Label", Description = "AgreeToTerms_Hint", ResourceType = typeof(Forms))]
        public bool AgreeToTerms { get; set; }



        [Required]
        [Display(Name = "Interests_Label", ResourceType = typeof(Forms))]
        public IEnumerable<string> SelectedInterests { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<SelectListItem> InterestOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "sports", Text = "Sports" },
            new SelectListItem { Value = "music", Text = "Music" },
            new SelectListItem { Value = "travel", Text = "Travel" },
            new SelectListItem { Value = "reading", Text = "Reading" }
        };

    }
}
