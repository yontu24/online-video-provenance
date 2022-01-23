using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InitialDatasetService
{
    public class GlobalVariables
    {
        public static List<string> datasetFields = new List<string>() {
            "genre", "abstract" , "budget", "cinematography","director", "language", "producer", "starring",
            "runtime", "writer","wikipageid", "editor", "musicComposer", "shortMovie", "distribuitor", "name"
        };

        public static string resourcesPrefix = "http://www.wade-ovi.org/resources#";
        
        public static string dbpediaEndpointUrl = "http://live.dbpedia.org/sparql";
    }
}
