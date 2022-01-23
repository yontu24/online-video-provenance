using VDS.RDF.Query;

namespace QueryBuilderLibrary.Interfaces
{
    public interface ISparqlEndpointConnection
    {
        public SparqlRemoteEndpoint GetConnection();

        public SparqlResultSet RunQuery(string query);
    }
}
