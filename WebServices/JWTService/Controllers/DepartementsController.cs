using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTService.Models
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="admin")]
    public class DepartementsController : ControllerBase
    {
        private IContext _context;

        public DepartementsController(IContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Departement>Get()
        {
            return _context.Departements;
        }
        [HttpGet("{id}")]
        public Departement Get(int id)
        {
           var current = _context.Departements.Where(d=>d.Id == id).FirstOrDefault(); 
            return current;
        }

        [HttpPost]
        public void Add([FromBody]Departement department) {
            _context.Add(department);
        }

        [HttpPut]
        public void Update(int id,[FromBody] Departement departement)
        {
            var current = Get(id);
            current = departement;
        }

        [HttpDelete]
        public void Remove([FromBody] Departement departement)
        {
            _context.Remove(departement);
        }

    }
}
