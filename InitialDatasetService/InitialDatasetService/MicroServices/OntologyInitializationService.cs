using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace InitialDatasetService.MicroServices
{
    public class OntologyInitializationService
    {
        public static void initializeOntology()
        {

            //Here we use an imaginary example file Ontology.rdf - substitute in an appropriate filename
            IGraph g = new Graph();
            FileLoader.Load(g, "Ontology.ttl");

            //Get the Node representing the class of Interest
            //Again we use an imaginary class URI, substitute in an appropriate URI
            /*INode someClass = g.GetUriNode(new Uri("http://www.wade-ovi.org/Movie"));

            //Note - GetUriNode returns null if no such URI exists, make sure to check for this or use
            //CreateUriNode instead
            if (someClass == null) return;
            Console.WriteLine("ceva\n");
            //Write out the Super Classes
            INode subClassOf = g.CreateUriNode(new Uri(NamespaceMapper.RDFS + "subClassOf"));
            foreach (Triple t in g.GetTriplesWithSubjectPredicate(someClass, subClassOf))
            {
                Console.WriteLine("Super Class: " + t.Object.ToString());
            }
            //Write out the Sub Classes
            foreach (Triple t in g.GetTriplesWithPredicateObject(subClassOf, someClass))
            {
                Console.WriteLine("Sub Class: " + t.Subject.ToString());
            }
            */

            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"));
            endpoint.ResultsAcceptHeader = "application/sparql-results+json";
            endpoint.Timeout = 300000; // 5 mins
            /*
            SparqlRemoteEndpoint endpointRepo = new SparqlRemoteEndpoint(new Uri("http://localhost:8080/rdf4j-server/repositories/wade1"));
            endpointRepo.ResultsAcceptHeader = "application/sparql-results+json";
            endpointRepo.Timeout = 300000; // 5 mins
            */
            SesameHttpProtocolConnector connector = new SesameHttpProtocolConnector("http://localhost:8080/rdf4j-server", "wade1") { Timeout = int.MaxValue };
            connector.SaveGraph(g);
            
            List<Triple> triples = new();
            List<string> infoToPull = new List<string>() { "genre", "abstract" , "budget", "cinematography",
                "director", "language", "producer", "starring", "runtime", "writer"};

            /*
             *
             *  also, maximum query paramsm aside from dbo:film and schema:CreativeWork seems to be 10
             *  , "distributor"
                "gross" - not working
                ?movie dbp:{infoToPull[11]} ?{infoToPull[11]} 
             * 
             */
            
            string selectVariables = "?movie " + String.Join(" ", infoToPull.Select(x => $"Group_Concat(distinct ?{x}, ', ') as ?{x}"));

            string queryString = $@"                  
                    SELECT {selectVariables} WHERE {{
                        ?movie a dbo:Film .
                        ?movie a schema:CreativeWork ;
                            dbo:{infoToPull[0]} ?{infoToPull[0]} ; 
                            dbo:{infoToPull[1]} ?{infoToPull[1]} ;
                            dbp:{infoToPull[2]} ?{infoToPull[2]} ;
                            dbp:{infoToPull[3]} ?{infoToPull[3]} ;
                            dbp:{infoToPull[4]} ?{infoToPull[4]} ;
                            dbp:{infoToPull[5]} ?{infoToPull[5]} ;
                            dbp:{infoToPull[6]} ?{infoToPull[6]} ; 
                            dbp:{infoToPull[7]} ?{infoToPull[7]} ;
                            dbp:{infoToPull[8]} ?{infoToPull[8]} ;
                            dbp:{infoToPull[9]} ?{infoToPull[9]} 
                    }} LIMIT 5";

            SparqlResultSet results = endpoint.QueryWithResultSet(queryString);
            
            var prefix = "http://www.wade-ovi.org/resources/";
            var uri = new Uri(prefix);
            g.NamespaceMap.AddNamespace("resources", uri);
            Dictionary<string, Dictionary<string, List<string>>> movieInfo = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (var result in results)
            {
                
                var title = g.CreateUriNode(new Uri(prefix + result.Value("movie").ToString().Split("/").Last()));
                var genre = g.CreateUriNode(new Uri(prefix + result.Value("genre").ToString().Split("/").Last()));
                var abstr = g.CreateUriNode(new Uri(prefix + result.Value("abstract").ToString()));
                var budget = g.CreateUriNode(new Uri(prefix + result.Value("budget").ToString()));
                var director = g.CreateUriNode(new Uri(prefix + result.Value("director").ToString().Split("/").Last()));
                var language = g.CreateUriNode(new Uri(prefix + result.Value("language").ToString()));
                var producer = g.CreateUriNode(new Uri(prefix + result.Value("producer").ToString().Split("/").Last()));
                var cinematography = g.CreateUriNode(new Uri(prefix + result.Value("cinematography").ToString()));
                var writer = g.CreateUriNode(new Uri(prefix + result.Value("writer").ToString().Split("/").Last()));
                var runtime = g.CreateUriNode(new Uri(prefix + result.Value("runtime").ToString()));
                var starring = g.CreateUriNode(new Uri(prefix + result.Value("starring").ToString().Split(",").Last()));

                var subject = g.CreateUriNode(new Uri(prefix + "movie/" + result.Value("movie").ToString().Split("/").Last()));

                triples.Add(new Triple(subject, g.CreateUriNode("resources:title"), title));
                triples.Add(new Triple(subject, g.CreateUriNode("resources:starring"), starring));
                triples.Add(new Triple(subject, g.CreateUriNode("resources:budget"), budget));
                triples.Add(new Triple(subject, g.CreateUriNode("resources:runtime"), runtime));
                triples.Add(new Triple(subject, g.CreateUriNode("resources:abstract"), abstr));
                triples.Add(new Triple(subject, g.CreateUriNode("resources:genre"), genre));
                triples.Add(new Triple(subject, g.CreateUriNode("resources:language"), language));
                triples.Add(new Triple(subject, g.CreateUriNode("resources:directedBy"), director));
                triples.Add(new Triple(subject, g.CreateUriNode("resources:producedBy"), producer));
                triples.Add(new Triple(subject, g.CreateUriNode("resources:writtenBy"), writer));
            }

            connector.UpdateGraph(prefix, triples,null);

            foreach (var result in results)
            {
                var title = result.Value("movie").ToString();
                List<string> infoReturned = new List<string>();

                for (int i = 0; i < infoToPull.Count; ++i)
                {
                    infoReturned.Add(result.Value(infoToPull[i]).ToString());
                }

                if (!movieInfo.ContainsKey(title))
                    movieInfo[title] = new Dictionary<string, List<string>>();

                for (int i = 0; i < infoToPull.Count; ++i)
                {
                    if (!movieInfo[title].ContainsKey(infoToPull[i]))
                        movieInfo[title][infoToPull[i]] = new List<string>();

                    movieInfo[title][infoToPull[i]].Add(infoReturned[i]);
                }
            }
            
            List<string> output = results.ToList().Select(x => x.ToString()).ToList();
            
        }
    
    }
}
