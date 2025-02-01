using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektWypozyczalnia.Models;

namespace ProjektWypozyczalnia.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProjektWypozyczalnia.Models.Car> Car { get; set; } = default!;
        public DbSet<ProjektWypozyczalnia.Models.CarType> CarType { get; set; } = default!;
        public DbSet<ProjektWypozyczalnia.Models.Manufacturer> Manufacturer { get; set; } = default!;
        public DbSet<ProjektWypozyczalnia.Models.Model> Models { get; set; } = default!;
        public DbSet<ProjektWypozyczalnia.Models.Reservation> Reservation { get; set; } = default!;
        public DbSet<ProjektWypozyczalnia.Models.Status> Status { get; set; } = default!;
        


    }
}
