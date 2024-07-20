using System;
using System.Collections.Generic;
using System.Linq;
namespace MySoapService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class GreetingService : IGreetingService
    {
        public string greeting(string name)
        {
            return $"Hello {name} it's {DateTime.Now.Hour}:{DateTime.Now.Minute} now";
        }
    }
}
