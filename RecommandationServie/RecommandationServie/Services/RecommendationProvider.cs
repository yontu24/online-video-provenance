using QueryBuilderLibrary.Implementations;
using RecommandationServie.Helpers;
using RecommandationServie.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace RecommandationServie.Services
{
    public class RecommendationProvider : IRecommendationProvider
    {
        public SparqlResultSet GetRecommendationByActors(List<string> actors)
        {
            throw new NotImplementedException();
        }

        public SparqlResultSet GetRecommendationByGenre(string genre)
        {
            SparqlEndpointConnection connection = new SparqlEndpointConnection(CommonVariables.EndpointConnectionUri);
            QueryBuilder queryBuilder = new QueryBuilder();

            queryBuilder
                .DeclarePrefix(CommonVariables.ResourcesPrefix, CommonVariables.ResourcesPrefixUri.OriginalString)
                .AddAggregatedSubject("name")
                .AddSubject("movie")
                .WithSubjectOfType("Movie")
                .AddTriple("title", "name")
                .AddTriple("genre", "genre")
                .AddStringFilter("genre", genre)
                .AddLimit(10);

            string query = queryBuilder.BuildQuery();

            return connection.RunQuery(query);
        }
    }
}
