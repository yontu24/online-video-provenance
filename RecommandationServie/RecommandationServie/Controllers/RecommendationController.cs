using Microsoft.AspNetCore.Mvc;
using RecommandationServie.Interfaces;

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
    }
}
