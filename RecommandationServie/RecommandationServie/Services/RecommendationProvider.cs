using QueryBuilderLibrary.Implementations;
using RecommandationService.Helpers;
using RecommandationService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace RecommandationService.Services
{
    public class RecommendationProvider : IRecommendationProvider
    {
        public SparqlResultSet GetRecommendationByDirectors(string movieUri, List<string> directors)
        {
            SparqlEndpointConnection connection = new SparqlEndpointConnection(CommonVariables.EndpointConnectionUri);
            QueryBuilder queryBuilder = new QueryBuilder();

            queryBuilder
                .DeclarePrefix(CommonVariables.ResourcesPrefix, CommonVariables.ResourcesPrefixUri.OriginalString)
                .AddAggregatedSubject("name")
                .AddSubject("movie")
                .WithSubjectOfType("Movie")
                .AddTriple("title", "name")
                .AddTriple("directedBy", "director")
                .AddMultipleValuesFilter("director", directors)
                .AddFilter($"?movie not in (<{movieUri}>)")
                .AddLimit(10);
            string query = queryBuilder.BuildQuery();

            return connection.RunQuery(query);
        }
        public SparqlResultSet GetRecommendationByGenres(string movieUri, List<string> genres)
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
                .AddMultipleValuesFilter("genre", genres, true)
                .AddFilter($"?movie not in (<{movieUri}>)")
                .AddLimit(10);
            string query = queryBuilder.BuildQuery();

            return connection.RunQuery(query);
        }

        public SparqlResultSet GetRecommendationByActors(string movieUri, List<string> actors)
        {
            SparqlEndpointConnection connection = new SparqlEndpointConnection(CommonVariables.EndpointConnectionUri);
            QueryBuilder queryBuilder = new QueryBuilder();

            queryBuilder
                .DeclarePrefix(CommonVariables.ResourcesPrefix, CommonVariables.ResourcesPrefixUri.OriginalString)
                .AddAggregatedSubject("name")
                .AddSubject("movie")
                .WithSubjectOfType("Movie")
                .AddTriple("title", "name")
                .AddTriple("starring", "actor")
                .AddMultipleValuesFilter("actor", actors)
                .AddFilter($"?movie not in (<{movieUri}>)")
                .AddLimit(10);
            string query = queryBuilder.BuildQuery();

            return connection.RunQuery(query);
        }
    }
}
