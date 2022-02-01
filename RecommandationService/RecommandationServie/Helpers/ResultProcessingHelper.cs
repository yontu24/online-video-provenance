using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace RecommandationService.Helpers
{
    public class ResultProcessingHelper
    {
        public static  Dictionary<string, List<string>> ProcessRecomandationsResult(SparqlResultSet results)
        {
            var processedRecomandation = new Dictionary<string, List<string>>();
            foreach (var result in results)
            {
                var movie = result.Value("movie").ToString();
                var titles = new List<string>();
                foreach (var name in result.Value("name").ToString().Split(CommonVariables.separator))
                {
                    titles.Add(WebUtility.UrlDecode(name));
                }
                processedRecomandation[movie] = titles;

            }
            return processedRecomandation;
        }
    }
}
