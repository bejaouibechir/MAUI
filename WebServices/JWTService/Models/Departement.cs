namespace JWTService.Models
{
    public class Departement
    {

        public Departement()
        {
              Employees= new HashSet<Employee>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Employee> Employees { get; set; }    
    }
}
