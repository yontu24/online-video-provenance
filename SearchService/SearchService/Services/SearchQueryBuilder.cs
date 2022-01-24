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

        public string GetMoviesFromDbpediaByTitle(string title)
        {
            //will be created with library functions at a later time when
            //i figure out how to do it properly
            string queryString = $@"                  
                    SELECT ?movie ?prop (GROUP_CONCAT(distinct ?value; SEPARATOR = ', ') as ?value) WHERE {{
                        {{select distinct ?movie WHERE{{?movie a dbo:Film. ?movie a schema:CreativeWork .
                        ?movie dbp:director ?director.
                        ?movie dbp:language ?language.
                        ?movie dbp:starring ?starring.
                        ?movie dbp:name ?name.
                        filter( regex(lcase(str(?name)), '{title}'))
                        }}  limit 10}}
                        
                        
                        ?movie ?prop ?value.
                        filter( ?prop not in (rdf:type))
                            
                    }}";
            return queryString;
        }

        public string GetPersonDetailsFromDbpediaByName(string name)
        {
            //will be created with library functions at a later time when
            //i figure out how to do it properly
            string queryString = $@"                  
                    SELECT ?person ?prop (GROUP_CONCAT(distinct ?value; SEPARATOR = ', ') as ?value) WHERE {{
                        {{select distinct ?person WHERE{{?person a dbo:Person. 
                        ?person dbp:name ?name.
                        filter( regex(lcase(str(?name)), '{name.ToLower()}'))
                        }}  limit 10}}
                        
                        
                        ?person ?prop ?value.
                        filter( ?prop not in (rdf:type))
                            
                    }}";
            return queryString;
        }
    }
}
