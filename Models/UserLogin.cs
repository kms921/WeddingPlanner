using System; 
using System.ComponentModel.DataAnnotations;


namespace WeddingPlanner.Models 
{
    public class UserLogin
    {   
        [Key] 
        [EmailAddress]
        [Required]
        public string Email {get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string  Password {get; set; }





    }





}