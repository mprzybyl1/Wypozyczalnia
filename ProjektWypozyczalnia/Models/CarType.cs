namespace ProjektWypozyczalnia.Models
{
    public class CarType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
