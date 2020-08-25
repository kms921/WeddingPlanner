using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace WeddingPlanner.Models
{
    public class WeddingGuest
    {
        [Key]
        public int WeddingGuestId {get; set;}
        public int UserId {get; set; }
        public int WeddingId {get; set; }
        public User User {get; set; }
        public Wedding Wedding {get; set; }
        public DateTime CreatedAt {get; set; }
        public DateTime UpdatedAt   {get; set;}
    }
}
