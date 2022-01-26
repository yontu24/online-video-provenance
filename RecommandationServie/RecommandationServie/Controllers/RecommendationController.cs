using Microsoft.AspNetCore.Mvc;
using RecommandationServie.Interfaces;
using System.Collections.Generic;

namespace RecommandationServie.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationProvider _recommendationProvider;

        public RecommendationController(IRecommendationProvider recommendationProvider)
        {
            _recommendationProvider = recommendationProvider;
        }

        [HttpGet("/genre/{genre}")]
        public IActionResult RecommendBasedOnGenre(string genre)
        {


            return Ok(_recommendationProvider.GetRecommendationByGenre(genre));
        }
        [HttpGet("/recomandation")]
        public IActionResult RecommendBasedOnParams()
        {
            string genre = Request.Query["genres"];
            string director = Request.Query["directors"];
            string actors = Request.Query["actors"];

            return Ok(_recommendationProvider.GetRecommendationByGenre(genre));
        }
    }
}
