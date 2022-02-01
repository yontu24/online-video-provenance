using QueryBuilderLibrary.Implementations;
using SearchService.Helpers;
using SearchService.Interfaces;
using System;
using System.Net;

namespace SearchService.Services
{
    
    public class SearchQueryBuilder : ISearchQueryBuilder
    {
        
        public string GetMatchingTitles(string title)
        {

            QueryBuilder queryBuilder = new QueryBuilder();
            title = title.Trim().Replace(" ", "\\\\+");
            queryBuilder
                .DeclarePrefix(CommonVariables.ResourcesPrefix, CommonVariables.ResourcesPrefixUri.OriginalString)
                .AddAggregatedSubject("name")
                .AddSubject(CommonVariables.CommonSubjects[0])
                .WithSubjectOfType("Movie")
                .AddTriple("title", "name")
                .AddStringFilter("name", title);

            return queryBuilder.BuildQuery();
        }

        public string GetMovieInfoByTitle(string movieUri)
        {
            QueryBuilder queryBuilder = new QueryBuilder();
            var decodedUri = WebUtility.UrlDecode(movieUri).Trim().Replace(" ", "+");
            queryBuilder
                .DeclarePrefix(CommonVariables.ResourcesPrefix, CommonVariables.ResourcesPrefixUri.OriginalString)
                .AddMultipleSubjects(CommonVariables.CommonSubjects)
                .AddAggregatedSubject("name")
                .AddAggregatedSubject("value")
                .UseSubject(CommonVariables.CommonSubjects[0]) // "movie"
                .WithSubjectOfType("Movie")
                .AddTriple("title", "name")
                .AddFilter($"?movie = (<{decodedUri}>)");
            // some weird error here where i could not chain the newly added AddTriple method
            queryBuilder
                .AddTriple("?movie ?prop ?value.")
                .AddFilter("( ?prop not in (rdf:type))");

            return queryBuilder.BuildQuery();
        }

        public string GetMoviesFromDbpediaByTitle(string title)
        {
            QueryBuilder innerQueryBuilder = new QueryBuilder();
            innerQueryBuilder
               .AddDistinctSubject("movie")
               .WithSubjectOfType("dbo", "Film")
               .WithSubjectOfType("schema", "CreativeWork")
               .UsePrefix("dbp")
               .AddTriple("director", "director")
               .AddTriple("language", "language")
               .AddTriple("starring", "starring")
               .AddTriple("name", "name")
               .AddStringFilter("name", $@"{title.ToLower()}")
               .AddLimit(10);

            QueryBuilder outerQueryBuilder = new QueryBuilder();
            outerQueryBuilder
                .AddSubject("movie")
                .AddSubject("prop")
                .AddAggregatedSubject("value");
            outerQueryBuilder
                .AddTriple($" {{ {innerQueryBuilder.BuildQuery(true)} }}");
            outerQueryBuilder
                .AddTriple("?movie ?prop ?value.")
                .AddFilter("?prop not in (rdf:type)");

            return outerQueryBuilder.BuildQuery(true);
        }

        public string GetPersonDetailsFromDbpediaByUri(string uri)
        {
            QueryBuilder innerQueryBuilder = new QueryBuilder();
            innerQueryBuilder
               .AddDistinctSubject("person")
               .WithSubjectOfType("dbo", "Person")
               .UsePrefix("dbp")
               .AddTriple("name", "name")
               .AddFilter($" ?person = <{uri}> ")
               .AddLimit(10);

            QueryBuilder outerQueryBuilder = new QueryBuilder();
            outerQueryBuilder
                .AddSubject("person")
                .AddSubject("prop")
                .AddAggregatedSubject("value");
            outerQueryBuilder
                .AddTriple($" {{ {innerQueryBuilder.BuildQuery(true)} }}");
            outerQueryBuilder
                .AddTriple("?person ?prop ?value.")
                .AddFilter("?prop not in (rdf:type)");

            return outerQueryBuilder.BuildQuery(true);
        }

        public string GetResourceByUri(string uri)
        {
            var filter = $" ?subject = <{uri}> || ?predicate = <{uri}> || ?object = <{uri}> ";
            QueryBuilder queryBuilder = new QueryBuilder();
            queryBuilder
                .AddSubject("subject")
                .AddSubject("predicate")
                .AddSubject("object");
            queryBuilder
                .AddTriple("?subject ?predicate ?object.")
                .AddFilter(filter);

            return queryBuilder.BuildQuery(true);

        }
    }
}
