using Microsoft.EntityFrameworkCore;

namespace ApiProjectCamp.WebAPI.Context
{
    public class ApiContext:DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=Mert;initial catalog=ApiYummyDb;integrated security=true;trust server certificate=true");
        }

        public DbSet<Entities.Category> Categories { get; set; }
        public DbSet<Entities.Chef> Chefs { get; set; }
        public DbSet<Entities.Contact> Contacts { get; set; }
        public DbSet<Entities.Feature> Features { get; set; }
        public DbSet<Entities.Image> Images { get; set; }
        public DbSet<Entities.Message> Messages { get; set; }
        public DbSet<Entities.Product> Products { get; set; }
        public DbSet<Entities.Reservation> Reservations { get; set; }
        public DbSet<Entities.Service> Services { get; set; }
        public DbSet<Entities.Testimonial> Testimonials { get; set; }
        public DbSet<Entities.YummyEvent> YummyEvents { get; set; }
        public DbSet<Entities.Notification> Notifications { get; set; }
        public DbSet<Entities.About> Abouts { get; set; }







    }
}
