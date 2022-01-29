using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace RecommandationService.Interfaces
{
    public interface IRecommendationProvider
    {
        public SparqlResultSet GetRecommendationByGenres(string movieUri, List<string> genres);

        public SparqlResultSet GetRecommendationByActors(string movieUri, List<string> actors);
        public SparqlResultSet GetRecommendationByDirectors(string movieUri, List<string> directors);
    }
}
