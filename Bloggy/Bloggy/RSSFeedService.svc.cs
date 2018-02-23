using Bloggy.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bloggy
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "RSSFeedService" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez RSSFeedService.svc ou RSSFeedService.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class RSSFeedService : IRSSFeedService
    {
        public List<Article> getLastArticles(int number)
        {
            var context = new ApplicationDbContext();
            return context.Articles.Take(number).ToList();
        }
    }
}
