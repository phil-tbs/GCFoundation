using GCFoundation.Components.Attributes;
using GCFoundation.Components.Models;
using GCFoundation.Web.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GCFoundation.Web.Models
{
    /// <summary>
    /// ViewModel for testing form validation with multiple input types.
    /// </summary>
    public class FormTestViewModel : BaseViewModel
    {
        /// <summary>
        /// The full name of the user.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FullName_Label", ResourceType = typeof(Forms))]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// The user's email address.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email_Label", ResourceType = typeof(Forms))]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password_Label", ResourceType = typeof(Forms))]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The user's website URL.
        /// </summary>
        [Required]
        [DataType(DataType.Url, ErrorMessageResourceType = typeof(Forms), ErrorMessageResourceName = "InvalidUrl")]
        [Display(Name = "Website_Label", ResourceType = typeof(Forms))]
        public string Website { get; set; } = string.Empty;

        /// <summary>
        /// The user's age. Must be between 18 and 100.
        /// </summary>
        [Required]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
        [Display(Name = "Age_Label", ResourceType = typeof(Forms))]
        public int? Age { get; set; }

        /// <summary>
        /// The user's date of birth.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "DateOfBirth_Label", Description = "DateOfBirth_Hint", ResourceType = typeof(Forms))]
        [DateFormat("full")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// A short biography of the user.
        /// </summary>
        [Required]
        [MinLength(10)]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Bio_Label", Description = "Bio_Hint", ResourceType = typeof(Forms))]
        public string Bio { get; set; } = string.Empty;

        /// <summary>
        /// The country selected by the user.
        /// </summary>
        [Required]
        [Display(Name = "Country_Label", Description = "Country_Hint", ResourceType = typeof(Forms))]
        public string? SelectedCountry { get; set; }

        /// <summary>
        /// Available country options.
        /// </summary>
        public IEnumerable<SelectListItem> CountryOptions { get; set; } =
        [
            new() { Value = "CA", Text = "Canada" },
            new() { Value = "US", Text = "United States" },
            new() { Value = "FR", Text = "France" },
            new() { Value = "DE", Text = "Germany" }
        ];

        /// <summary>
        /// The gender selected by the user.
        /// </summary>
        [Required]
        [Display(Name = "Gender_Label", ResourceType = typeof(Forms))]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Available gender options.
        /// </summary>
        public IEnumerable<SelectListItem> GenderOptions { get; set; } =
        [
            new() { Value = "Male", Text = "Male" },
            new() { Value = "Female", Text = "Female" },
            new() { Value = "Other", Text = "Other" }
        ];

        /// <summary>
        /// Indicates whether the user agrees to the terms.
        /// </summary>
        [Required]
        [Display(Name = "AgreeToTerms_Label", Description = "AgreeToTerms_Hint", ResourceType = typeof(Forms))]
        public bool AgreeToTerms { get; set; }


        /// <summary>
        /// The list of interests selected by the user.
        /// </summary>
        [Required]
        [Display(Name = "Interests_Label", ResourceType = typeof(Forms))]
        public IEnumerable<string> SelectedInterests { get; set; } = [];

        /// <summary>
        /// Available interest options.
        /// </summary>
        public IEnumerable<SelectListItem> InterestOptions { get; set; } =
        [
            new() { Value = "sports", Text = "Sports" },
            new() { Value = "music", Text = "Music" },
            new() { Value = "travel", Text = "Travel" },
            new() { Value = "reading", Text = "Reading" }
        ];

    }
}
