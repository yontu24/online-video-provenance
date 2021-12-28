using Microsoft.AspNetCore.Mvc;
using QueryBuilderService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF.Query;

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
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"));
            endpoint.ResultsAcceptHeader = "application/sparql-results+json";
            endpoint.Timeout = 300000; // 5 mins
            string query = _queryBuilder.MainInfoQuery();
            SparqlResultSet results = endpoint.QueryWithResultSet(query);

            return Ok(results.ToList());
        }

        [HttpGet("{movie}")]
        public IActionResult GetSepcificMovieInfo(string movie)
        {
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"));
            endpoint.ResultsAcceptHeader = "application/sparql-results+json";
            endpoint.Timeout = 300000; // 5 mins
            string query = _queryBuilder.GetSpecificMovieInfo(movie);
            SparqlResultSet results = endpoint.QueryWithResultSet(query);

            return Ok(results.ToList().Select(x => x.ToString()));
        }
    }
}
