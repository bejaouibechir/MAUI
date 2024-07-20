using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MyServiceGraphQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepController : ControllerBase
    {
        private static List<Department> _departments = new List<Department>
        {
            new Department { Id = "1", Name = "Engineering", Location = "Headquarters", Manager = "John Smith" },
            new Department { Id = "2", Name = "Marketing", Location = "Branch Office", Manager = "Emily Johnson" },
            new Department { Id = "3", Name = "Sales", Location = "Main Office", Manager = "Michael Brown" },
            new Department { Id = "4", Name = "Human Resources", Location = "Regional Office", Manager = "Sarah Davis" }
        };

        [HttpGet]
        public IEnumerable<Department> Get()
        {
            return _departments;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
                return NotFound();
            return Ok(department);
        }
    }
}
