using JWTService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="admin,moderator")]
    public class UsersController : ControllerBase
    {
        private IContext _context;

        public UsersController(IContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            var cookie = new CookieOptions
            {
                // Spécifie le domaine de la cookie
                Domain = "https://localhost:7187",
                // Spécifie le chemin de la cookie (optionnel)
                Path = "/",
                // Définit si la cookie est accessible uniquement via HTTP (false par défaut)
                HttpOnly = false,
                // Définit si la cookie est sécurisée (false par défaut)
                Secure = false,
                // Définit si la cookie est accessible uniquement via HTTPS (false par défaut)
                SameSite = SameSiteMode.None,
                // Durée de vie de la cookie (optionnel)
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            };
            // Ajouter la cookie à la réponse HTTP
            Response.Cookies.Append("MonCookie", "ValeurDuCookie", cookie);

            return _context.Employees;
        }
        [HttpGet("{id}")]
        public Employee Get(int id)
        {
           var current = _context.Employees.Where(e=>e.Id == id).FirstOrDefault();
           return current;
        }

        [HttpPost]
        public void Add([FromBody] Employee employee)
        {
            _context.Add(employee);
        }

        [HttpPut]
        public void Update(int id, [FromBody] Employee employee)
        {
           var current = Get(id);
            current = employee;

        }

        [HttpDelete]
        public void Remove([FromBody] Employee employee)
        {
            _context.Remove(employee);
        }

    }
}
