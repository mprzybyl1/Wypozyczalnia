using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjektWypozyczalnia.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Nazwa producenta nie może przekraczać 50 znaków.")]
        [Display(Name = "Nazwa producenta", Description = "Podaj nazwę producenta samochodów.")]
        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
