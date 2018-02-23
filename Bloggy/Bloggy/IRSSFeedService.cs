using Bloggy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Bloggy
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IRSSFeedService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IRSSFeedService
    {
        [OperationContract]
        List<Article> getLastArticles(int number);
    }
}
