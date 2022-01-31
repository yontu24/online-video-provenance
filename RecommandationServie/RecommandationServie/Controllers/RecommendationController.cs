using Microsoft.AspNetCore.Mvc;
using RecommandationService.Helpers;
using RecommandationService.Interfaces;
using System.Collections.Generic;
using System.Net;

namespace RecommandationService.Controllers
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

        [HttpGet("/recommendation")]
        public IActionResult RecommendBasedOnParams()
        {
            string genresInput = Request.Query["genres"];
            string movieInput = Request.Query["movie"];
            string directorsInput = Request.Query["directors"];
            string actorsInput = Request.Query["actors"];
            List<string> actors = new List<string>() , genres = new List<string>(), directors = new List<string>();
            string movie = "";
            if(movieInput != null)
            {
                movie = WebUtility.UrlDecode(movieInput).Trim().Replace(" ", "+");
            }
            if(genresInput != null)
            {
                
                foreach (var genre in genresInput.Split(CommonVariables.separator))
                {
                    genres.Add(WebUtility.UrlDecode(genre));
                }
            }
            if(directorsInput != null)
            {
                
                foreach (var director in directorsInput.Split(CommonVariables.separator))
                {
                    directors.Add(WebUtility.UrlDecode(director));
                }
            }
            if(actorsInput != null)
            {
                
                foreach (var actor in actorsInput.Split(CommonVariables.separator))
                {
                    actors.Add(WebUtility.UrlDecode(actor));
                }
            }
            
            var processedRecomandation = new Dictionary<string, Dictionary<string, List<string>>> ();
            var actorsRecommendations = actors.Count == 0 ? null :_recommendationProvider.GetRecommendationByActors(movie, actors);
            var genresRecommendations = genres.Count == 0 ? null : _recommendationProvider.GetRecommendationByGenres(movie, genres);
            var directorsRecommendations = directors.Count == 0 ? null : _recommendationProvider.GetRecommendationByDirectors(movie, directors);
           
            processedRecomandation["actors"] = actorsRecommendations == null? null : ResultProcessingHelper.ProcessRecomandationsResult(actorsRecommendations);
            processedRecomandation["genres"] = genresRecommendations == null ? null : ResultProcessingHelper.ProcessRecomandationsResult(genresRecommendations);
            processedRecomandation["directors"] = directorsRecommendations == null ? null : ResultProcessingHelper.ProcessRecomandationsResult(directorsRecommendations);
            return Ok(processedRecomandation);
        }
    }
}
