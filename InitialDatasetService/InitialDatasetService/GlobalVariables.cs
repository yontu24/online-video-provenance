using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InitialDatasetService
{
    public class GlobalVariables
    {
        public static List<string> DatasetFields = new List<string>() {
            "genre", "abstract" , "budget", "cinematography","director", "language", "producer", "starring",
            "runtime", "writer","wikipageid", "editor", "musicComposer", "shortMovie", "distribuitor", "name"
        };

        public static string ResourcesPrefix = "http://www.wade-ovi.org/resources#";
        
        public static string DBpediaEndpointUrl = "http://live.dbpedia.org/sparql";

        public static string RdfSyntaxUri = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
    }
}
