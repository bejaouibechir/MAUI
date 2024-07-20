namespace JWTService.Models
{
    public class Context : IContext
    {
        public Context()
        {
            Departements = new List<Departement>
            {
                new Departement { Id = 1, Name = "Finance" },
                new Departement { Id = 2, Name = "Marketing" },
                new Departement { Id = 3, Name = "Production" }
            };

            Employees = new List<Employee>
            {
                new Employee
                {
                    Id = 100,
                    Login = "jhon",
                    Password = "aaa",
                    Role = "admin",
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Phone = "1234567890",
                    HireTime = DateTime.Now.AddDays(-100),
                    Salary = 50000,
                    Departement = Departements[0]
                },
                new Employee
                {
                    Id = 101,
                    Login = "jhon1",
                    Password = "aaa",
                    Role = "moderator",
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Phone = "1234567890",
                    HireTime = DateTime.Now.AddDays(-100),
                    Salary = 50000,
                    Departement = Departements[0]
                },
                new Employee
                {
                    Id = 102,
                    Login = "john2",
                    Password = "aaa",
                    Role = "user",
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Phone = "1234567890",
                    HireTime = DateTime.Now.AddDays(-100),
                    Salary = 50000,
                    Departement = Departements[0]
                },
                new Employee
                {
                    Id = 2,
                    Login = "employee2",
                    Password = "password2",
                    Role = "user",
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Phone = "0987654321",
                    HireTime = DateTime.Now.AddDays(-150),
                    Salary = 60000,
                    Departement = Departements[1]
                },
                new Employee
                {
                    Id = 3,
                    Login = "employee3",
                    Password = "password3",
                    Role = "user",
                    Name = "Michael Johnson",
                    Email = "michael.johnson@example.com",
                    Phone = "5432167890",
                    HireTime = DateTime.Now.AddDays(-200),
                    Salary = 70000,
                    Departement = Departements[2]
                },
                new Employee
                {
                    Id = 4,
                    Login = "employee4",
                    Password = "password4",
                    Role = "user",
                    Name = "Emily Brown",
                    Email = "emily.brown@example.com",
                    Phone = "9876543210",
                    HireTime = DateTime.Now.AddDays(-250),
                    Salary = 55000,
                    Departement = Departements[0]
                },
                new Employee
                {
                    Id = 5,
                    Login = "employee5",
                    Password = "password5",
                    Role = "user",
                    Name = "William Williams",
                    Email = "william.williams@example.com",
                    Phone = "0123456789",
                    HireTime = DateTime.Now.AddDays(-300),
                    Salary = 65000,
                    Departement = Departements[1]
                },
                new Employee
                {
                    Id = 6,
                    Login = "employee6",
                    Password = "password6",
                    Role = "user",
                    Name = "Emma Garcia",
                    Email = "emma.garcia@example.com",
                    Phone = "5432109876",
                    HireTime = DateTime.Now.AddDays(-350),
                    Salary = 60000,
                    Departement = Departements[2]
                },
                new Employee
                {
                    Id = 7,
                    Login = "employee7",
                    Password = "password7",
                    Role = "user",
                    Name = "Alexander Taylor",
                    Email = "alexander.taylor@example.com",
                    Phone = "6549873210",
                    HireTime = DateTime.Now.AddDays(-400),
                    Salary = 55000,
                    Departement = Departements[0]
                },
                new Employee
                {
                    Id = 8,
                    Login = "employee8",
                    Password = "password8",
                    Role = "user",
                    Name = "Sophia Martinez",
                    Email = "sophia.martinez@example.com",
                    Phone = "7894561230",
                    HireTime = DateTime.Now.AddDays(-450),
                    Salary = 70000,
                    Departement = Departements[1]
                },
                new Employee
                {
                    Id = 9,
                    Login = "employee9",
                    Password = "password9",
                    Role = "user",
                    Name = "James Clark",
                    Email = "james.clark@example.com",
                    Phone = "1597534680",
                    HireTime = DateTime.Now.AddDays(-500),
                    Salary = 50000,
                    Departement = Departements[2]
                },
                new Employee
                {
                    Id = 10,
                    Login = "employee10",
                    Password = "password10",
                    Role = "user",
                    Name = "Olivia Jones",
                    Email = "olivia.jones@example.com",
                    Phone = "3571594862",
                    HireTime = DateTime.Now.AddDays(-550),
                    Salary = 65000,
                    Departement = Departements[0]
                },
                new Employee
                {
                    Id = 11,
                    Login = "employee11",
                    Password = "password11",
                    Role = "user",
                    Name = "Michael Williams",
                    Email = "michael.williams@example.com",
                    Phone = "6541239870",
                    HireTime = DateTime.Now.AddDays(-600),
                    Salary = 60000,
                    Departement = Departements[1]
                },
                new Employee
                {
                    Id = 12,
                    Login = "employee12",
                    Password = "password12",
                    Role = "user",
                    Name = "Jane Garcia",
                    Email = "jane.garcia@example.com",
                    Phone = "3216549870",
                    HireTime = DateTime.Now.AddDays(-650),
                    Salary = 55000,
                    Departement = Departements[2]
                },
                new Employee
                {
                    Id = 13,
                    Login = "employee13",
                    Password = "password13",
                    Role = "user",
                    Name = "William Brown",
                    Email = "william.brown@example.com",
                    Phone = "4567891230",
                    HireTime = DateTime.Now.AddDays(-700),
                    Salary = 70000,
                    Departement = Departements[0]
                },
                new Employee
                {
                    Id = 14,
                    Login = "employee14",
                    Password = "password14",
                    Role = "user",
                    Name = "Emily Taylor",
                    Email = "emily.taylor@example.com",
                    Phone = "9876543210",
                    HireTime = DateTime.Now.AddDays(-750),
                    Salary = 50000,
                    Departement = Departements[1]
                },
                new Employee
                {
                    Id = 15,
                    Login = "employee15",
                    Password = "password15",
                    Role = "user",
                    Name = "John Smith",
                    Email = "john.smith@example.com",
                    Phone = "1234567890",
                    HireTime = DateTime.Now.AddDays(-800),
                    Salary = 65000,
                    Departement = Departements[2]
                },
                new Employee
                {
                    Id = 16,
                    Login = "employee16",
                    Password = "password16",
                    Role = "user",
                    Name = "Sophia Clark",
                    Email = "sophia.clark@example.com",
                    Phone = "9876543210",
                    HireTime = DateTime.Now.AddDays(-850),
                    Salary = 60000
                },
                new Employee
                {
                    Id = 17,
                    Login = "employee17",
                    Password = "password17",
                    Role = "user",
                    Name = "Michael Garcia",
                    Email = "michael.garcia@example.com",
                    Phone = "3216549870",
                    HireTime = DateTime.Now.AddDays(-900),
                    Salary = 55000,
                    Departement = Departements[1]
                }
            };

            Weathers = new List<Weather> {
               new Weather {Id=1, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(12)), Summary = "Hot", TemperatureC = 35 },
            new Weather { Id=2, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(13)), Summary = "Sunny", TemperatureC = 28 },
            new Weather { Id=3, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(14)), Summary = "Cloudy", TemperatureC = 20 },
             new Weather { Id=4, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(12)), Summary = "Hot", TemperatureC = 35 },
            new Weather { Id=5, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(13)), Summary = "Sunny", TemperatureC = 24 },
            new Weather { Id=6, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(14)), Summary = "Cloudy", TemperatureC = 20 },
             new Weather { Id=7, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(12)), Summary = "Hot", TemperatureC = 36 },
            new Weather { Id=8, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(13)), Summary = "Sunny", TemperatureC = 29 },
            new Weather { Id=9, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(14)), Summary = "Cloudy", TemperatureC = 20 },
             new Weather { Id=10, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(12)), Summary = "Hot", TemperatureC = 47 },
            new Weather { Id=11, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(13)), Summary = "Sunny", TemperatureC = 29 },
            new Weather { Id=12, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(14)), Summary = "Cloudy", TemperatureC = 19 },
             new Weather { Id=13, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(12)), Summary = "Hot", TemperatureC = 35 },
            new Weather { Id=14, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(13)), Summary = "Sunny", TemperatureC = 27 },
            new Weather { Id=15, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(14)), Summary = "Cloudy", TemperatureC = 22 }
            };
        }
        public List<Employee> Employees { get; set; }
        public List<Departement> Departements { get; set; }

        public List<Weather> Weathers { get; set; }

        public void Add<T>(T entity)
        {
            if (entity is Employee) Employees.Add(entity as Employee);
            else if (entity is Departement) Departements.Add(entity as Departement);

        }

        public void Remove<T>(T entity)
        {
            if (entity is Employee) Employees.Remove(entity as Employee);
            else if (entity is Departement) Departements.Remove(entity as Departement);
        }

        public void Update<T>(int id, T entity)
        {
            if (entity is Employee)
            {
                Employees.RemoveAt(id);
                Employees.Add(entity as Employee);
            }

            if (entity is Departement)
            {
                Departements.RemoveAt(id);
                Departements.Add(entity as Departement);
            }

        }

    }
}
