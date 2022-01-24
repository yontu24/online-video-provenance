using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace RecommandationServie.Interfaces
{
    public interface IRecommendationProvider
    {
        public SparqlResultSet GetRecommendationByGenre(string genre);

        public SparqlResultSet GetRecommendationByActors(List<string> actors);
    }
}
