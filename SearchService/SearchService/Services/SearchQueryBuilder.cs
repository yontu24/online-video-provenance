using QueryBuilderLibrary.Implementations;
using SearchService.Helpers;
using SearchService.Interfaces;
using System;

namespace SearchService.Services
{
    public class SearchQueryBuilder : ISearchQueryBuilder
    {
        public string GetMatchingTitles(string title)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            queryBuilder
                .DeclarePrefix(CommonVariables.ResourcesPrefix, CommonVariables.ResourcesPrefixUri.OriginalString)
                .AddAggregatedSubject("name")
                .AddSubject(CommonVariables.CommonSubjects[0])
                .WithSubjectOfType("Movie")
                .AddTriple("title", "name")
                .AddStringFilter("name", title);

            return queryBuilder.BuildQuery();
        }

        public string GetMovieInfoByTitle(string title)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            queryBuilder
                .DeclarePrefix(CommonVariables.ResourcesPrefix, CommonVariables.ResourcesPrefixUri.OriginalString)
                .AddMultipleSubjects(CommonVariables.CommonSubjects)
                .AddAggregatedSubject("name")
                .AddAggregatedSubject("value")
                .UseSubject(CommonVariables.CommonSubjects[0]) // "movie"
                .WithSubjectOfType("Movie")
                .AddTriple("title", "name")
                .AddStringFilter("name", title.ToLower());
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
               .SetSeparator(", ")
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
                .SetSeparator(", ")
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

        public string GetPersonDetailsFromDbpediaByName(string name)
        {
            QueryBuilder innerQueryBuilder = new QueryBuilder();
            innerQueryBuilder
               .SetSeparator(", ")
               .AddDistinctSubject("person")
               .WithSubjectOfType("dbo", "Person")
               .UsePrefix("dbp")
               .AddTriple("name", "name")
               .AddStringFilter("name", $@"{name.ToLower()}")
               .AddLimit(10);

            QueryBuilder outerQueryBuilder = new QueryBuilder();
            outerQueryBuilder
                .SetSeparator(", ")
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
    }
}
