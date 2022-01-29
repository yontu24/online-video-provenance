﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("/movies/titles/{title}")]
        public IActionResult GetMatchingTitles(string title)
        {
            SparqlEndpointConnection endpointConnection = new SparqlEndpointConnection(new Uri("http://localhost:8080/rdf4j-server/repositories/wade1"));
            string query = _queryBuilder.GetMatchingTitles(title);

            return Ok(ResultProcessingHelper.ProcessTitlesResult(endpointConnection.RunQuery(query)));
        }

        [HttpGet("/movies/data/{movieUri}")]
        public IActionResult GetMovieInfoByTitle(string movieUri)
        {
            SparqlEndpointConnection endpointConnection = new SparqlEndpointConnection(new Uri("http://localhost:8080/rdf4j-server/repositories/wade1"));
            string query = _queryBuilder.GetMovieInfoByTitle(movieUri);

            return Ok(ResultProcessingHelper.ProcessInfoResult("movie", endpointConnection.RunQuery(query)));
        }

        [HttpGet("/dbpedia/movies/{title}")]
        public IActionResult GetMoviesFromDbpediaByTitle(string title)
        {
            SparqlEndpointConnection endpointConnection = new SparqlEndpointConnection(new Uri("http://live.dbpedia.org/sparql"));
            string query = _queryBuilder.GetMoviesFromDbpediaByTitle(title);
            var processedResults = ResultProcessingHelper.ProcessInfoResult("movie", endpointConnection.RunQuery(query));
            DatasetUpdateHelper.UpdateDatasetMovies(processedResults);
            return Ok(processedResults);
        }

        [HttpGet("/dbpedia/persons/{name}")]
        public IActionResult GetPersonsFromDbpediaByName(string name)
        {
            SparqlEndpointConnection endpointConnection = new SparqlEndpointConnection(new Uri("http://live.dbpedia.org/sparql"));
            string query = _queryBuilder.GetPersonDetailsFromDbpediaByName(name);
            var processedResults = ResultProcessingHelper.ProcessInfoResult("person", endpointConnection.RunQuery(query));
             DatasetUpdateHelper.UpdateDatasetPersons(processedResults);
            return Ok(processedResults);
        }
    }
}
