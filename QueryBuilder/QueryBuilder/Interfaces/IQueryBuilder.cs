using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryBuilderService.Interfaces
{
    public interface IQueryBuilder
    {
        public List<string> Prefixes { get; set; }

        public string BuildQuery();

        public string MainInfoQuery();

        public string GetMovieInfoByTitle(string title);

        public string GetMatchingTitles(string title);
    }
}
