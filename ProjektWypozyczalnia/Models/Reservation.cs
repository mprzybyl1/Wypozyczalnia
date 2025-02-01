using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjektWypozyczalnia.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Pojazd", Description = "Wybierz pojazd do rezerwacji.")]
        public int CarId { get; set; }

        public Car? Car { get; set; }

        [Required]
        [Display(Name = "Użytkownik", Description = "ID użytkownika dokonującego rezerwacji.")]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        [Required]
        [Display(Name = "Status rezerwacji", Description = "Określ status rezerwacji.")]
        public int StatusId { get; set; }

        public Status? Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data rozpoczęcia", Description = "Wybierz datę rozpoczęcia rezerwacji.")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data zakończenia", Description = "Wybierz datę zakończenia rezerwacji.")]
        public DateTime EndDate { get; set; }
    }
}
