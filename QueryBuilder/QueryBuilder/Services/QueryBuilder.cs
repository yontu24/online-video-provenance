using QueryBuilderService.Entities;
using QueryBuilderService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderService.Services
{
    public class QueryBuilder : IQueryBuilder
    {
        #region Fields

        protected List<string> prefixes;
        protected List<string> mainInfo = new List<string>() { "genre", "abstract" , "budget", "cinematography",
                "director", "language", "producer", "starring", "runtime", "writer"};
        protected Dictionary<string, List<String>> prefixVariablePairs = new Dictionary<string, List<string>>()
            { 
                {"dbo", new List<string>(){
                        "genre", "abstract"//, "wikiPageID"
                    }
                },
                {"dbp", new List<string>(){
                        "budget", "cinematography", "director", "language", "producer", "starring", "runtime", "writer", "name"
                    }
                }
            };
        protected string separator = "|split|";


        #endregion

        #region Properties

        public List<string> Prefixes
        {
            get => prefixes;
            set
            {
                if (value.Any(x => !Prefix.GetAll().Contains(x)))
                    return;

                prefixes = value;
            }
        }

        #endregion

        public string BuildQuery()
        {   
            return "";
        }

        public string MainInfoQuery()
        {
            string subject = GetSubject(mainInfo);
            StringBuilder queryString = new StringBuilder();
            queryString.AppendLine($"SELECT ?movie {subject} WHERE {{ ?movie a dbo:Film. ?movie a schema:CreativeWork;");

            foreach(string key in prefixVariablePairs.Keys)
                foreach(string variable in prefixVariablePairs[key])
                    queryString.AppendLine($"{key}:{variable} ?{variable};");

            // FILTER regex(str(?name), \"Friends\")
            queryString.AppendLine(/*"FILTER regex(str(?name), \"Friends\")*/"} Limit 100");
            string returnString = queryString.ToString();
            int idx = returnString.LastIndexOf(';');

            return returnString.Remove(idx, 1);
        }

        public string GetMovieInfoByTitle(string title)
        {
            return GetMovieInfoByTitleOwn(title);

            List<string> propSubjList = new List<string>() { "name", "value" };
            string subject = GetSubject(propSubjList);
            StringBuilder queryString = new StringBuilder();
            
            queryString
                .AppendLine($"SELECT ?movie ?prop {subject} WHERE {{ ?movie a dbo:Film. ?movie a schema:CreativeWork;")
                .AppendLine("dbp:name ?name.")
                .AppendLine($"filter( regex(lcase(str(?name)), \"{title}\"))")
                // .AppendLine($"filter( regex(lcase(str(?name)), \"^{title}$\"))")
                .AppendLine("?movie ?prop ?value.")
                .AppendLine("filter( ?prop not in (rdf:type))")
                .AppendLine("} LIMIT 1000");

            return queryString.ToString();
        }

        public string GetMatchingTitles(string title)
        {
            return GetMatchingTitlesOwn(title);

            List<string> propSubjList = new List<string>() { "name" };
            string subject = GetSubject(propSubjList);
            StringBuilder queryString = new StringBuilder();
            
            queryString
                .AppendLine($"SELECT ?movie {subject} WHERE {{ ?movie a dbo:Film. ?movie a schema:CreativeWork;")
                .AppendLine("dbp:name ?name.")
                .AppendLine($"filter( regex(lcase(str(?name)), \"{title}\"))")
                .AppendLine("} ");

            return queryString.ToString();
        }

        public string GetMovieInfoByTitleOwn(string title)
        {
            title = WebUtility.UrlDecode(title);
            List<string> propSubjList = new List<string>() { "name", "value" };
            string subject = GetSubject(propSubjList);
            StringBuilder queryString = new StringBuilder();
            queryString
                .AppendLine("PREFIX resources: <http://www.wade-ovi.org/resources#>")
                .AppendLine($"SELECT ?movie ?prop {subject} WHERE {{ ?movie a resources:Movie;")
                .AppendLine("resources:title ?name.")
                // .AppendLine($"filter( regex(lcase(str(?name)), \"{title}\"))")
                .AppendLine($"filter( regex(lcase(str(?name)), \"^{title}$\"))")
                .AppendLine("?movie ?prop ?value.")
                .AppendLine("filter( ?prop not in (rdf:type))")
                .AppendLine("} GROUP BY ?movie ?prop");

            return queryString.ToString();
        }

        public string GetMatchingTitlesOwn(string title)
        {
            /*
                PREFIX resources: <http://www.wade-ovi.org/resources#>
                SELECT ?movie (GROUP_CONCAT(distinct ?name;SEPARATOR = "|split|") as ?title) WHERE { 
                    ?movie a resources:Movie;
                    resources:title ?name.
                    filter( regex(lcase(str(?name)), "megafault"))
                }
             */
            List<string> propSubjList = new List<string>() { "name" };
            string subject = GetSubject(propSubjList);
            StringBuilder queryString = new StringBuilder();
            queryString
                .AppendLine("PREFIX resources: <http://www.wade-ovi.org/resources#>")
                .AppendLine($"SELECT ?movie {subject} WHERE {{ ?movie a resources:Movie;")
                .AppendLine("resources:title ?name.")
                .AppendLine($"filter( regex(lcase(str(?name)), \"{title}\"))")
                .AppendLine("} GROUP BY ?movie");

            return queryString.ToString();
        }

        private string GetSubject(List<string> info)
        {
            return String.Join(" ", info.Select(x => $"(GROUP_CONCAT(distinct ?{x}; SEPARATOR = \"{separator}\") as ?{x})"));
        }
    }
}
