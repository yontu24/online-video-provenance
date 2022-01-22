using Microsoft.AspNetCore.Mvc;
using QueryBuilderLibrary.Implementations;
using System;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchServiceController : ControllerBase
    {
        public SearchServiceController()
        {

        }

        [HttpGet("/getTitle/{title}")]
        public IActionResult GetMatchingTitles(string title)
        {
            QueryBuilder queryBuilder = new QueryBuilder();
            SparqlEndpointConnection endpointConnection = new SparqlEndpointConnection(new Uri("http://localhost:8080/rdf4j-server/repositories/wade1"));

            return Ok();
        }
    }
}
