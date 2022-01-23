using Microsoft.AspNetCore.Mvc;
using QueryBuilderLibrary.Implementations;
using SearchService.Helpers;
using SearchService.Interfaces;
using System;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchServiceController : ControllerBase
    {
        protected readonly ISearchQueryBuilder _queryBuilder;

        public SearchServiceController(ISearchQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        [HttpGet("/getTitles/{title}")]
        public IActionResult GetMatchingTitles(string title)
        {
            SparqlEndpointConnection endpointConnection = new SparqlEndpointConnection(new Uri("http://localhost:8080/rdf4j-server/repositories/wade1"));
            string query = _queryBuilder.GetMatchingTitles(title);

            return Ok(ResultProcessingHelper.ProcessTitlesResult(endpointConnection.RunQuery(query)));
        }

        [HttpGet("/getTitleInfo/{title}")]
        public IActionResult GetMovieInfoByTitle(string title)
        {
            SparqlEndpointConnection endpointConnection = new SparqlEndpointConnection(new Uri("http://localhost:8080/rdf4j-server/repositories/wade1"));
            string query = _queryBuilder.GetMovieInfoByTitle(title);

            return Ok(ResultProcessingHelper.ProcessMovieInfoResult(endpointConnection.RunQuery(query)));
        }
    }
}
