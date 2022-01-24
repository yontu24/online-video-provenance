using Microsoft.AspNetCore.Mvc;
using QueryBuilderService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace QueryBuilderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryBuilderController : ControllerBase
    {
        protected readonly IQueryBuilder _queryBuilder;

        public QueryBuilderController(IQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        [HttpGet]
        public IActionResult GetQuery()
        {
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://live.dbpedia.org/sparql"));
            endpoint.ResultsAcceptHeader = "application/sparql-results+json";
            endpoint.Timeout = 300000; // 5 mins
            string query = _queryBuilder.MainInfoQuery();
            SparqlResultSet results = endpoint.QueryWithResultSet(query);

            return Ok(results.ToList());
        }

        [HttpGet("/getTitles/{title}")]
        public IActionResult GetMatchingTitles(string title)
        {
            // SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"));
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://localhost:8080/rdf4j-server/repositories/wade1"));
            endpoint.ResultsAcceptHeader = "application/sparql-results+json";
            endpoint.Timeout = 300000;
            string query = _queryBuilder.GetMatchingTitles(title.ToLower());
            SparqlResultSet results = endpoint.QueryWithResultSet(query);

            // ProcessResult(results);

            return Ok(ProcessTitleResult(results));
        }

        [HttpGet("/getMovieInfo/{title}")]
        public IActionResult GetMovieInfoByTitle(string title)
        {
            // SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"));
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://localhost:8080/rdf4j-server/repositories/wade1"));
            endpoint.ResultsAcceptHeader = "application/sparql-results+json";
            endpoint.Timeout = 300000; // 5 mins
            string query = _queryBuilder.GetMovieInfoByTitle(title.ToLower());
            SparqlResultSet results = endpoint.QueryWithResultSet(query);

            // ProcessResult(results);

            return Ok(ProcessInfoResult(results));
                // Ok(results.ToList().Select(x => x.ToString()));
        }

        private Dictionary<string, string> ProcessTitleResult(SparqlResultSet results)
        {
            Dictionary<string, string> movieInfo = new Dictionary<string, string>();

            foreach (var result in results)
            {
                var movie = result.Value("movie").ToString();
                movieInfo[movie] = result.Value("name").ToString();
             }

            return movieInfo;
        }

        private Dictionary<string, Dictionary<string, List<string>>> ProcessInfoResult(SparqlResultSet results)
        {
            Dictionary<string, Dictionary<string, List<string>>> movieInfo = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach(var result in results)
            {
                var movie = result.Value("movie").ToString();
                if (!movieInfo.ContainsKey(movie))
                    movieInfo[movie] = new Dictionary<string, List<string>>();

                movieInfo[movie][result.Value("prop").ToString()] = result.Value("value").ToString().Split("|split|").ToList();

               // movieInfo[result.Value("prop").ToString()] = result.Value("value").ToString().Split("|split|").ToList();              
            }

            return movieInfo;
        }
    }
}
