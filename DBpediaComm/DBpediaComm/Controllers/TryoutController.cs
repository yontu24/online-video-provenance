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
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetResults()
        {
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"));
            endpoint.ResultsAcceptHeader = "application/sparql-results+json";

            SparqlResultSet results = endpoint.QueryWithResultSet(@"    
                    PREFIX omc: <omc.ttl>
                    PREFIX cw: <cw.ttl>
                
                    SELECT * WHERE {
                        ?movie a dbo:Film .
                        ?movie a schema:CreativeWork .
                        ?movie dbo:genre ?genre . 
                    } LIMIT 100");

            Dictionary<string, List<string>> movieGenres = new Dictionary<string, List<string>>();

            foreach(var result in results)
            {
                var title = result.Value("movie").ToString();
                var genre = result.Value("genre").ToString();
                if (!movieGenres.ContainsKey(title))
                    movieGenres[title] = new List<string>();

                movieGenres[title].Add(genre);
            }

            List<string> output = results.ToList().Select(x => x.ToString()).ToList();

            TryoutCreateFile.CreateFile(movieGenres);
            TryoutCreateFile.QueryFile();

            return output;
        }
    }
}
