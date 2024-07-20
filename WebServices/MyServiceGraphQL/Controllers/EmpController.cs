using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MyServiceGraphQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        private static List<Employee> _employees = new List<Employee>
        {
            new Employee { Id = "3", Name = "Michael Johnson", Age = 35, Email = "michael.johnson@example.com", Address = "789 Elm St, Othertown, USA", DepartmentId = "3", JobTitle = "Sales Manager" },
            new Employee { Id = "9", DepartmentId = "1" },
            new Employee { Id = "4", Name = "Magda Steawart", Age = 30, Email = "magda.steawart@example.com", Address = "214 Pine St, TinaTawon, USA", DepartmentId = "4", JobTitle = "HR Specialist" },
            new Employee { Id = "5", Name = "Jhon Smith", Age = 35, Email = "john.smith@example.com", Address = "215 Pine St, TinaTawon, USA", DepartmentId = "2", JobTitle = "HR Specialist" },
            new Employee { Id = "6", Name = "Jhon Doe", Age = 35, Email = "john.doe@example.com", Address = "215 Pine St, TinaTawon, USA", DepartmentId = "1", JobTitle = "HR Specialist" }
        };

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _employees;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }
    }
}
