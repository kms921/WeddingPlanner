using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WeddingPlanner.Models 
{
     public class Future: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime EnteredDate = Convert.ToDateTime(value);
            if (EnteredDate > DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Wedding date must be in the future");
            }
        }
    }
    public class Wedding
    { 
        [Key]
        public int WeddingId {get; set;}
        [Required(ErrorMessage= "Wedder One is required.")]
        [Display(Name = "Wedder One: ")]

        public string WedderOne {get; set;}
        [Display(Name = "Wedder Two: ")]
        [Required(ErrorMessage= "Wedder Two is required.")]
        public string WedderTwo {get; set;}
        [Required(ErrorMessage= "Wedding Date is required.")]
        [Future]
        [DataType(DataType.Date)]
        [Display(Name = "Wedding Date: ")]

        public DateTime WeddingDate {get; set; }
        [Required(ErrorMessage = "Wedding Address is required")]
        [Display(Name = "Wedding Address: ")]
        public string Address { get; set; }
        public List<WeddingGuest> Guests {get; set; }

    }

}
