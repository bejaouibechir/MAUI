
namespace JWTService.Models
{
    public interface IContext
    {
        List<Departement> Departements { get; set; }
        List<Employee> Employees { get; set; }
        List<Weather> Weathers { get; set; }

        void Add<T>(T entity);
        void Remove<T>(T entity);
        void Update<T>(int id, T entity);
    }
}