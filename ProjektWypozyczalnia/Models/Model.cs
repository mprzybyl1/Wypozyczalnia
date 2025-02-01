using Microsoft.EntityFrameworkCore;

namespace ProjektWypozyczalnia.Models
{
    public class Model
    {
        public int Id { get; set; }
        [Precision(18, 2)]
        public decimal PricePerDay { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
