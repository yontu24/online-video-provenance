using QueryBuilderLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace QueryBuilderLibrary.Implementations
{
    public class SparqlEndpointConnection : ISparqlEndpointConnection
    {
        private SparqlRemoteEndpoint _endpoint;

        public SparqlEndpointConnection(string address)
        {
            _endpoint = new SparqlRemoteEndpoint(new Uri(address));
            BasicEndpointConfiguration();
        }

        public SparqlEndpointConnection(Uri uri)
        {
            _endpoint = new SparqlRemoteEndpoint(uri);
            BasicEndpointConfiguration();
        }

        private void BasicEndpointConfiguration()
        {
            _endpoint.ResultsAcceptHeader = "application/sparql-results+json";
            _endpoint.Timeout = 300000;
        }

        public SparqlRemoteEndpoint GetConnection() => _endpoint;

        // could also add the query function on this
    }
}
