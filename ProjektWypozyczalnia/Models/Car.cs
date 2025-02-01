using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjektWypozyczalnia.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Numer rejestracyjny nie może przekraczać 15 znaków.")]
        [Display(Name = "Numer rejestracyjny", Description = "Wpisz unikalny numer rejestracyjny pojazdu.")]
        public string RegistrationNumber { get; set; }

        [Required]
        [Display(Name = "Model pojazdu", Description = "Wybierz model pojazdu z listy.")]
        public int ModelId { get; set; }

        public Model? Model { get; set; }

        [Required]
        [Display(Name = "Typ pojazdu", Description = "Określ typ pojazdu (np. SUV, sedan, hatchback).")]
        public int TypeId { get; set; }

        public CarType? Type { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
