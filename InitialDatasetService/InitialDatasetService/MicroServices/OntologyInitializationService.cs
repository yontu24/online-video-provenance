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
        static string wordDelimiter = ", ";
        static string uriDelimiter = "http://dbpedia.org/resource/";

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

            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://live.dbpedia.org/sparql"));
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

            var prefix = "http://www.wade-ovi.org/resources#";
            g.NamespaceMap.AddNamespace("resources", new Uri(prefix));
            g.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            
            foreach (var result in results)
            {
                // subject
                var subject = g.CreateUriNode($"resources:movie/{result.Value("movie").ToString().Split("/").Last()}");
                triples.Add(new Triple(subject, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Movie")));

                // title
                foreach (var name in ExtractNames(result.Value("movie")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:title"), node));
                }

                // abstract
                foreach (var name in ExtractNames(result.Value("abstract")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:abstract"), node));
                }

                // budget
                foreach (var name in ExtractNames(result.Value("budget")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:budget"), node));
                }

                // director
                foreach (var name in ExtractNames(result.Value("director")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:directedBy"), node));
                    triples.Add(new Triple(node, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Director")));
                }

                // runtime
                foreach (var name in ExtractNames(result.Value("runtime")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:runtime"), node));
                }

                // language
                foreach (var name in ExtractNames(result.Value("language")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:language"), node));
                }

                // producer
                foreach (var name in ExtractNames(result.Value("producer")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:producedBy"), node));
                    triples.Add(new Triple(node, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Producer")));
                }

                // cinematography
                foreach (var name in ExtractNames(result.Value("cinematography")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:cinematography"), node));
                }

                // writer
                foreach (var name in ExtractNames(result.Value("writer")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:writtenBy"), node));
                    triples.Add(new Triple(node, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Writer")));
                }

                // genre
                foreach (var name in ExtractNames(result.Value("genre")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:genre"), node));
                }

                // starring
                foreach (var name in ExtractNames(result.Value("starring")))
                {
                    var node = g.CreateUriNode(name);
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:starring"), node));
                    triples.Add(new Triple(node, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Actor")));
                }
            }

            connector.UpdateGraph(prefix, triples, null);

            Dictionary<string, Dictionary<string, List<string>>> movieInfo = new Dictionary<string, Dictionary<string, List<string>>>();
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

            tList<string> output = results.ToList().Select(x => x.ToString()).ToList();

        }

        private static List<string> ExtractNames(INode node)
        {
            var names = node.ToString().Split(wordDelimiter, StringSplitOptions.None);
            List<string> uriNodesAsStrings = new();
            foreach (var name in names)
            {
                string currNode = name.Contains("http") ?
                    $"resources:{name.Split(uriDelimiter, StringSplitOptions.None)[1]}" :
                    $"resources:{name}";

                uriNodesAsStrings.Add(currNode);
            }
            return uriNodesAsStrings;
        }
    }
}
