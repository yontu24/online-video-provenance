using QueryBuilderLibrary.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using VDS.RDF.Query;

namespace SearchService.Helpers
{
    public static class ResultProcessingHelper
    {
        public static Dictionary<string, dynamic> ProcessTitlesResult(SparqlResultSet results)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            return ProcessTitlesResult(results, queryBuilder.Separator);
        }

        public static Dictionary<string, dynamic> ProcessTitlesResult(SparqlResultSet results, string separator)
        {
            Dictionary<string, dynamic> movieInfo = new Dictionary<string, dynamic>();

            foreach (var result in results)
            {
                var movie = result.Value("movie").ToString();
                movieInfo[movie] = new Dictionary<string, string>() { { "name", result.Value("name").ToString().Split(separator).First() } };
            }

            return movieInfo;
        }

        public static Dictionary<string, Dictionary<string, Dictionary<string, string>>> ProcessInfoResult(string inputKey, SparqlResultSet results)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            return ProcessInfoResult(inputKey, results, queryBuilder.Separator);
        }

        public static Dictionary<string, Dictionary<string, Dictionary<string, string>>> ProcessInfoResult(string inputKey, SparqlResultSet results, string separator)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> movieInfo = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            Regex pattern = new Regex("[_+]");

            foreach (var result in results)
            {
                var movie = result.Value(inputKey).ToString();
                if (!movieInfo.ContainsKey(movie))
                    movieInfo[movie] = new Dictionary<string, Dictionary<string, string>>();

                string prop = result.Value("prop").ToString();
                List<string> value = result.Value("value").ToString().Split(separator).ToList();
                movieInfo[movie][prop] = new Dictionary<string, string>();

                if (!value.FirstOrDefault().Contains("http"))
                    foreach(string val in value)
                        movieInfo[movie][prop][prop] = val;
                else
                    foreach (string val in value)
                        if (val.Contains("http"))
                        {
                            string temp = WebUtility.UrlDecode(val).Split("#").Last().Split("/").Last();
                            temp = pattern.Replace(temp, " ");
                            movieInfo[movie][prop][val] = temp;
                        }
            }

            return movieInfo;
        }
    }
}
