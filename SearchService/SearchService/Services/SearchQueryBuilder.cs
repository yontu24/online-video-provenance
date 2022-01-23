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
                .AddStringFilter(title);
            // some weird error here where i could not chain the newly added AddTriple method
            queryBuilder
                .AddTriple("?movie ?prop ?value.")
                .AddFilter("( ?prop not in (rdf:type))");

            return queryBuilder.BuildQuery();
        }
    }
}
