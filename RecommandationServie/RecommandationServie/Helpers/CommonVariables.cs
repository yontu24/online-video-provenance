using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommandationServie.Helpers
{
    public static class CommonVariables
    {
        public static readonly Uri EndpointConnectionUri = new Uri("http://localhost:8080/rdf4j-server/repositories/wade1");
        public static readonly Uri ResourcesPrefixUri = new Uri("http://www.wade-ovi.org/resources#");
        public const string ResourcesPrefix = "resources";
    }
}
