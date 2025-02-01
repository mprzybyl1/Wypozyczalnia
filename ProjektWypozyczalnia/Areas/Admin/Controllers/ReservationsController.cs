using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektWypozyczalnia.Data;
using ProjektWypozyczalnia.Models;

namespace ProjektWypozyczalnia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")]

    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Reservations
        [Route("Admin/Reservations")]
        public async Task<IActionResult> Index(string? registrationNumber)
        {
            var cars = await _context.Car.ToListAsync();
            ViewData["Cars"] = cars;

            var reservations = _context.Reservation
                .Include(r => r.Car)
                .Include(r => r.Status)
                .Include(r => r.User) // Dołącz użytkownika
                .AsQueryable();

            if (!string.IsNullOrEmpty(registrationNumber))
            {
                reservations = reservations.Where(r => r.Car.RegistrationNumber == registrationNumber);
            }

            return View(await reservations.ToListAsync());
        }
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Reservation.Include(r => r.Car).Include(r => r.Status);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        // GET: Admin/Reservations/Details/5
        [Route("Admin/Reservations/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Car)
                .Include(r => r.Status)
                .Include(r => r.User) // Dołącz użytkownika
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Admin/Reservations/Create
        [Route("Admin/Reservations/Create")]
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car.Include(c => c.Model), "Id", "Model.Name");
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Description");
            return View();
        }

        // POST: Admin/Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Admin/Reservations/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,StatusId,CarId")] Reservation reservation)
        {
            if (reservation.StartDate > reservation.EndDate)
            {
                // Dodanie błędu do ModelState, aby wyświetlić komunikat o błędzie w widoku
                ModelState.AddModelError("StartDate", "Data początkowa nie może być późniejsza niż data końcowa.");
            }

                reservation.UserId = _userManager.GetUserId(User);
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", reservation.CarId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id", reservation.StatusId);
            return View(reservation);
        }

        // GET: Admin/Reservations/Edit/5
        [Route("Admin/Reservations/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", reservation.CarId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id", reservation.StatusId);
            return View(reservation);
        }

        // POST: Admin/Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Admin/Reservations/Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate,StatusId,CarId,UserId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", reservation.CarId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id", reservation.StatusId);
            return View(reservation);
        }

        // GET: Admin/Reservations/Delete/5
        [Route("Admin/Reservations/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Car)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Admin/Reservations/Delete/5
        [Route("Admin/Reservations/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
