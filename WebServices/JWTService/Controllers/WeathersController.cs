using JWTService.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWTService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeathersController : ControllerBase
    {
        private readonly IContext _context;

        public WeathersController(IContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Weather> Get()
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

            return _context.Weathers;
        }
        [HttpGet("{id}")]
        public Weather Get(int id)
        {
            var current = _context.Weathers.Where(e => e.Id == id).FirstOrDefault();
            return current;
        }

        [HttpPost]
        public void Add([FromBody] Weather Weather)
        {
            _context.Add(Weather);
        }

        [HttpPut]
        public void Update(int id, [FromBody] Weather Weather)
        {
            var current = Get(id);
            current = Weather;

        }

        [HttpDelete]
        public void Remove([FromBody] Weather Weather)
        {
            _context.Remove(Weather);
        }

    }
}
