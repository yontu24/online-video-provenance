using QueryBuilderLibrary.Implementations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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

        public static Dictionary<string, Dictionary<string, dynamic>> ProcessInfoResult(string inputKey, SparqlResultSet results)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            return ProcessInfoResult(inputKey, results, queryBuilder.Separator);
        }

        public static Dictionary<string, Dictionary<string, dynamic>> ProcessInfoResult(string inputKey, SparqlResultSet results, string separator)
        {
            Dictionary<string, Dictionary<string, dynamic>> movieInfo = new Dictionary<string, Dictionary<string, dynamic>>();
            Regex pattern = new Regex("[_+]");

            // - movie
            // -- prop
            // --- propUri : prop value 

            foreach (var result in results)
            {
                var movie = result.Value("movie").ToString();
                if (!movieInfo.ContainsKey(movie))
                    movieInfo[movie] = new Dictionary<string, dynamic>();

                string prop = result.Value("prop").ToString();
                List<string> value = result.Value("value").ToString().Split(separator).ToList();
                
                if (!value.FirstOrDefault().Contains("http"))
                {
                    movieInfo[movie][prop] = string.Join(", ", value);
                }
                else
                {
                    movieInfo[movie][prop] = new Dictionary<string, string>();
                    foreach (string val in value)
                    {
                        if (val.Contains("http"))
                        {
                            string temp = WebUtility.UrlDecode(val).Split("#").Last();
                            temp = pattern.Replace(temp, " ");
                            movieInfo[movie][prop][val] = temp;
                                // .Add(new Dictionary<string, string>() { { temp, val } });
                        }
                    }
                }
                // movieInfo[movie][result.Value("prop").ToString()] = result.Value("value").ToString().Split(separator).ToList();
            }

            return movieInfo;
        }
    }
}
