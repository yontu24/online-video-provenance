using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchService.Interfaces
{
    public interface ISearchQueryBuilder
    {
        public string GetMatchingTitles(string title);

        public string GetMovieInfoByTitle(string title);
        public string GetMoviesFromDbpediaByTitle(string title);

        public string GetPersonDetailsFromDbpediaByName(string name);
    }
}
