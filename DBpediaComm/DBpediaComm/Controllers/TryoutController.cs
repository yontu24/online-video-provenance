using DBpediaComm.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Query;

namespace DBpediaComm.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TryoutController
    {
        /*
         * Should only be used to reset the dataset
         * 
         * dbpediaQB.BuildSelectQuery(input)
         * dbpediaConn.ExecuteQuery()
         * odatasetQB.BuildCreateQuery(res)
         * odatasetConn.ExecuteQuery()
         * odatasetQB.BuildSelectQuery()
         * 
         */
        [HttpGet]
        public async Task<ActionResult<List<string>>> PopulateDataset()
        {

            TryoutCreateFile.QueryFile();

            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"));
            endpoint.ResultsAcceptHeader = "application/sparql-results+json";
            endpoint.Timeout = 300000; // 5 mins
           
            List<string> infoToPull = new List<string>() { "genre", "abstract" , "budget", "cinematography", 
                "director", "language", "producer", "starring", "runtime", "writer"};

            /*
             *
             *  also, maximum query paramsm aside from dbo:film and schema:CreativeWork seems to be 10
             *  , "distributor"
                "gross" - not working
                ?movie dbp:{infoToPull[11]} ?{infoToPull[11]} 
             * 
             */

            string selectVariables = "?movie " + String.Join(" ", infoToPull.Select(x => $"Group_Concat(distinct ?{x}, ', ') as ?{x}"));

            string queryString = $@"                  
                    SELECT {selectVariables} WHERE {{
                        ?movie a dbo:Film .
                        ?movie a schema:CreativeWork ;
                            dbo:{infoToPull[0]} ?{infoToPull[0]} ; 
                            dbo:{infoToPull[1]} ?{infoToPull[1]} ;
                            dbp:{infoToPull[2]} ?{infoToPull[2]} ;
                            dbp:{infoToPull[3]} ?{infoToPull[3]} ;
                            dbp:{infoToPull[4]} ?{infoToPull[4]} ;
                            dbp:{infoToPull[5]} ?{infoToPull[5]} ;
                            dbp:{infoToPull[6]} ?{infoToPull[6]} ; 
                            dbp:{infoToPull[7]} ?{infoToPull[7]} ;
                            dbp:{infoToPull[8]} ?{infoToPull[8]} ;
                            dbp:{infoToPull[9]} ?{infoToPull[9]} 

                    }} LIMIT 100";

            SparqlResultSet results = endpoint.QueryWithResultSet(queryString);
            
            Dictionary<string, Dictionary<string, List<string>>> movieInfo = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach(var result in results)
            {
                var title = result.Value("movie").ToString();
                List<string> infoReturned = new List<string>();

                for(int i = 0; i < infoToPull.Count; ++i)
                {
                    infoReturned.Add(result.Value(infoToPull[i]).ToString());
                }

                if (!movieInfo.ContainsKey(title))
                    movieInfo[title] = new Dictionary<string, List<string>>();
                
                for (int i = 0; i < infoToPull.Count; ++i)
                {
                    if (!movieInfo[title].ContainsKey(infoToPull[i]))
                        movieInfo[title][infoToPull[i]] = new List<string>();

                    movieInfo[title][infoToPull[i]].Add(infoReturned[i]);
                }
            }

            List<string> output = results.ToList().Select(x => x.ToString()).ToList();

            // TryoutCreateFile.CreateFile(movieInfo);
            // gotta modify this too
            //TryoutCreateFile.QueryFile();

            return output;
        }
    }
}
