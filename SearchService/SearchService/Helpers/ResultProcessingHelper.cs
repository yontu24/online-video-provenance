using QueryBuilderLibrary.Implementations;
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

        public static Dictionary<string, Dictionary<string, List<string>>> ProcessInfoResult(string inputKey, SparqlResultSet results)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            return ProcessInfoResult(inputKey, results, queryBuilder.Separator);
        }

        public static Dictionary<string, Dictionary<string, List<string>>> ProcessInfoResult(string inputKey, SparqlResultSet results, string separator)
        {
            Dictionary<string, Dictionary<string, List<string>>> info = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (var result in results)
            {
                var key = result.Value(inputKey).ToString();
                if (!info.ContainsKey(key))
                    info[key] = new Dictionary<string, List<string>>();

                info[key][result.Value("prop").ToString()] = result.Value("value").ToString().Split(separator).ToList();
            }

            return info;
        }
    }
}
