using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchService.Helpers
{
    public static class CommonVariables
    {
        public static readonly Uri EndpointConnectionUri = new Uri("http://localhost:8080/rdf4j-server/repositories/wade1");
        public static readonly Uri ResourcesPrefixUri = new Uri("http://www.wade-ovi.org/resources#"); 
        public const string ResourcesPrefix = "resources";
        public static readonly List<string> CommonSubjects = new List<string>() { "movie", "prop" };
        public static Dictionary<string, string> propertyParameters = new Dictionary<string, string>(){
                { "director", "directedBy" },
                { "editor", "editedBy" },
                { "producer", "producedBy" },
                { "distribuitor", "distributedBy" },
                { "writer", "writtenBy" },
                { "musicComposer", "musicBy" }
            };
        public static List<string> datasetFields = new List<string>() {
            "genre", "abstract" , "budget", "cinematography","director", "language", "producer", "starring",
            "runtime", "writer","wikipageid", "editor", "musicComposer", "shortMovie", "distribuitor", "name"
        };

        public static List<string> personAttributes = new List<string>() {
            "name", "child", "education", "nationality", "parent", "spouse", "profession", "sex",
            "birthYear", "birthPlace", "eyeColour", "hairColour"
        };
    }
}
