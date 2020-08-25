using System; 
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class IndexWrapper
    {
        public User FormModel { get; set; }
        public User User { get; set; }
        
        public List<User> TableModel { get; set; }
        public UserLogin UserLogin { get; set; }
        public UserLogin OneUserLogin { get; set;}
        public Wedding OneWedding {get; set;}
        public List<Wedding> AllWeddings {get; set;}
        public WeddingGuest WeddingGuest {get; set;}
        public List<WeddingGuest> AllWeddingGuests {get; set; }
        public int CurrentUser { get; set; }

    }
} 