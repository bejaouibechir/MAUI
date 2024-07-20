
namespace MySoapService
{using System;
   using System.ServiceModel;
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu
    // Refactoriser pour changer le nom d'interface "IService1" à la fois dans
    // le code et le fichier de configuration.
    [ServiceContract]
    public interface IGreetingService
    {
        [OperationContract]
        string greeting(string name);
    }
    
}
