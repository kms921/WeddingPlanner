using Microsoft.EntityFrameworkCore;


namespace WeddingPlanner.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
        
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins {get; set;}
        public DbSet<Wedding> Weddings {get; set; }
        public DbSet<WeddingGuest> WeddingGuests {get; set; }

     
    }
}
