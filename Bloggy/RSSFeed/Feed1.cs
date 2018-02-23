using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Text;

namespace RSSFeed
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Feed1" à la fois dans le code et le fichier de configuration.
    public class Feed1 : IFeed1
    {
        public SyndicationFeedFormatter CreateFeed()
        {
            // Créez un flux RSS.
            SyndicationFeed feed = new SyndicationFeed("Bloggy", "Articles feed", null);
            List<SyndicationItem> items = new List<SyndicationItem>();

            // Create a new Syndication Item.
            Bloggy.RSSFeedService feedService = new Bloggy.RSSFeedService();

            var list = feedService.getLastArticles(10);

            foreach (var itemArticle in list)
            {
                SyndicationItem item = new SyndicationItem(itemArticle.banner, itemArticle.description, new Uri("http://localhost:9569/Article/Read/" + itemArticle.id));
                items.Add(item);
            }

            feed.Items = items;

            // Renvoie ATOM ou RSS en fonction de la chaîne de requête
            // rss -> http://localhost:8733/Design_Time_Addresses/RSSFeed/Feed1/
            // atom -> http://localhost:8733/Design_Time_Addresses/RSSFeed/Feed1/?format=atom
            string query = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["format"];
            SyndicationFeedFormatter formatter = null;
            if (query == "atom")
            {
                formatter = new Atom10FeedFormatter(feed);
            }
            else
            {
                formatter = new Rss20FeedFormatter(feed);
            }

            return formatter;
        }
    }
}
