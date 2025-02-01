using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektWypozyczalnia.Data;
using ProjektWypozyczalnia.Models;

namespace ProjektWypozyczalnia.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles ="User,Admin")]
    
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: User/Reservations
        [Route("User/Reservations")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User); // Pobranie ID zalogowanego użytkownika

            var reservations = _context.Reservation
                .Include(r => r.Car)
                .Include(r => r.Status)
                .Include(r => r.User)
                .Where(r => r.UserId == userId) // Filtrowanie tylko dla zalogowanego użytkownika
                .ToListAsync();

            return View(await reservations);
        }


        // GET: User/Reservations/Details/5
        [Route("User/Reservations/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
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
            if (reservation.UserId != User.Identity.Name)
            {
                // Zwrócenie odpowiedzi 403 Forbidden lub przekierowanie na stronę błędu
                return Forbid(); // lub return RedirectToAction("AccessDenied", "Account");
            }

            return View(reservation);
        }

        // GET: User/Reservations/Create
        [Route("User/Reservations/Create")]
        public IActionResult Create(int carId)
        {
            var car = _context.Car.Include(c => c.Model).FirstOrDefault(c => c.Id == carId);

            if (car == null)
            {
                return NotFound();
            }

            ViewData["CarId"] = new SelectList(_context.Car.Include(c => c.Model), "Id", "Model.Name",car.Id);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Description");
            var reservation = new Reservation
            {
                CarId = car.Id // Przekazujemy carId do formularza rezerwacji
            };

            return View(reservation);
        }

        // POST: User/Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("User/Reservations/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,StatusId,CarId")] Reservation reservation)
        {

            reservation.UserId = _userManager.GetUserId(User);
            _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", reservation.CarId);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Id", reservation.StatusId);
            return View(reservation);
        }

        // GET: User/Reservations/Edit/5
        [Route("User/Reservations/Edit/{id}")]
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
            if (reservation.UserId != User.Identity.Name)
            {
                // Zwrócenie odpowiedzi 403 Forbidden lub przekierowanie na stronę błędu
                return Forbid(); // lub return RedirectToAction("AccessDenied", "Account");
            }
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", reservation.CarId);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Id", reservation.StatusId);
            return View(reservation);
        }

        // POST: User/Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("User/Reservations/Edit/{id}")]
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
                    if (reservation.UserId != User.Identity.Name)
                    {
                        // Zwrócenie odpowiedzi 403 Forbidden lub przekierowanie na stronę błędu
                        return Forbid(); // lub return RedirectToAction("AccessDenied", "Account");
                    }
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
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Id", reservation.StatusId);
            return View(reservation);
        }

        // GET: User/Reservations/Delete/5
        [Route("User/Reservations/Delete/{id}")]
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
            if (reservation.UserId != User.Identity.Name)
            {
                // Zwrócenie odpowiedzi 403 Forbidden lub przekierowanie na stronę błędu
                return Forbid(); // lub return RedirectToAction("AccessDenied", "Account");
            }

            return View(reservation);
        }

        // POST: User/Reservations/Delete/5
        [Route("User/Reservations/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }
            if (reservation.UserId != User.Identity.Name)
            {
                // Zwrócenie odpowiedzi 403 Forbidden lub przekierowanie na stronę błędu
                return Forbid(); // lub return RedirectToAction("AccessDenied", "Account");
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
