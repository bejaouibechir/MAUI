namespace JWTService.Models
{
    public class Employee:User
    {
        public string Name { get; set; }    
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime HireTime { get; set; }
        public decimal Salary { get; set;}
        public Departement Departement { get; set; }

    }
}
