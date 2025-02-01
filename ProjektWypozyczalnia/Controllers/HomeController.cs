using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektWypozyczalnia.Data;
using ProjektWypozyczalnia.Models;
using System.Diagnostics;



namespace ProjektWypozyczalnia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Home/Top5Cars
        
        public async Task<IActionResult> Index()
        {
            // Pobierz 5 samochodów z bazy danych
            var cars = await _context.Car
                .Include(c => c.Model)// Jeœli chcesz za³¹czyæ dane modelu
                .ThenInclude(m => m.Manufacturer) 
                .Include(c => c.Type)// Jeœli chcesz za³¹czyæ typ samochodu
                .Include(c => c.Reservations)       // Za³aduj rezerwacje
                .ThenInclude(r => r.Status)
                .Take(5)
                .ToListAsync();

            return View(cars);
        }


        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
