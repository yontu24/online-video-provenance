﻿using QueryBuilderLibrary.Implementations;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF.Query;

namespace SearchService.Helpers
{
    public static class ResultProcessingHelper
    {
        public static Dictionary<string, string> ProcessTitlesResult(SparqlResultSet results)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            return ProcessTitlesResult(results, queryBuilder.Separator);
        }

        public static Dictionary<string, string> ProcessTitlesResult(SparqlResultSet results, string separator)
        {
            Dictionary<string, string> movieInfo = new Dictionary<string, string>();

            foreach (var result in results)
            {
                var movie = result.Value("movie").ToString();
                movieInfo[movie] = result.Value("name").ToString();
            }

            return movieInfo;
        }

        public static Dictionary<string, Dictionary<string, List<string>>> ProcessMovieInfoResult(SparqlResultSet results)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            return ProcessMovieInfoResult(results, queryBuilder.Separator);
        }

        public static Dictionary<string, Dictionary<string, List<string>>> ProcessMovieInfoResult(SparqlResultSet results, string separator)
        {
            Dictionary<string, Dictionary<string, List<string>>> movieInfo = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (var result in results)
            {
                var movie = result.Value("movie").ToString();
                if (!movieInfo.ContainsKey(movie))
                    movieInfo[movie] = new Dictionary<string, List<string>>();

                movieInfo[movie][result.Value("prop").ToString()] = result.Value("value").ToString().Split(separator).ToList();
            }

            return movieInfo;
        }
    }
}
