using InitialDatasetService.Interfaces;
using QueryBuilderLibrary.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace InitialDatasetService.MicroServices
{
    public class DatasetInitialization : IDatasetInitialization
    {
        private static IGraph g = new Graph();
        private static List<Triple> triples = new();
        private static List<string> datasetFields = GlobalVariables.datasetFields;

        private static Dictionary<string, string> propertyParameters = new Dictionary<string, string>(){
                { "director", "directedBy" },
                { "editor", "editedBy" },
                { "producer", "producedBy" },
                { "distribuitor", "distributedBy" },
                { "writer", "writtenBy" },
                { "musicComposer", "musicBy" }
            };
        public void InitializeDataset()
        {
        
            FileLoader.Load(g, "Ontology.ttl");
            
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri(GlobalVariables.dbpediaEndpointUrl));
            endpoint.ResultsAcceptHeader = "application/sparql-results+json";
            endpoint.Timeout = 300000; // 5 mins
           
            SesameHttpProtocolConnector connector = new SesameHttpProtocolConnector("http://localhost:8080/rdf4j-server", "wade1") { Timeout = int.MaxValue };
            connector.SaveGraph(g);

            var personsWithNames = new List<string>() { "director" };
            
            for (var currentLetter = 'a'; currentLetter <= 'z'; currentLetter ++)
            {
                /*
                string queryString = $@"                  
                    SELECT ?movie ?prop (GROUP_CONCAT(distinct ?value; SEPARATOR = ', ') as ?value) WHERE {{
                        {{select distinct ?movie WHERE{{?movie a dbo:Film. ?movie a schema:CreativeWork .
                        ?movie dbp:director ?director.
                        ?movie dbp:language ?language.
                        ?movie dbp:starring ?starring.
                        ?movie dbp:name ?name.
                        filter( regex(lcase(str(?name)), '^{currentLetter}'))
                        }}  limit 10}}
                        
                        
                        ?movie ?prop ?value.
                        filter( ?prop not in (rdf:type))
                            
                    }}";
                */
               
                QueryBuilder innerQueryBuilder = new QueryBuilder();
                innerQueryBuilder
                   .SetSeparator(", ")
                   .AddDistinctSubject("movie")
                   .WithSubjectOfType("dbo", "Film")
                   .WithSubjectOfType("schema", "CreativeWork")
                   .UsePrefix("dbp")
                   .AddTriple("director", "director")
                   .AddTriple("language", "language")
                   .AddTriple("starring", "starring")
                   .AddTriple("name", "name")
                   .AddStringFilter("name", $@"^{currentLetter}")
                   .AddLimit(10);
                
                QueryBuilder outerQueryBuilder = new QueryBuilder();
                outerQueryBuilder
                    .SetSeparator(", ")
                    .AddSubject("movie")
                    .AddSubject("prop")
                    .AddAggregatedSubject("value");
                outerQueryBuilder
                    .AddTriple($" {{ {innerQueryBuilder.BuildQuery(true)} }}");  
                outerQueryBuilder
                    .AddTriple("?movie ?prop ?value.")
                    .AddFilter("?prop not in (rdf:type)");

                var finalQuery = outerQueryBuilder.BuildQuery(true);
               
                SparqlResultSet queryResult = endpoint.QueryWithResultSet(finalQuery);
                var processedResult = ProcessInfoResult(queryResult);

                
                g.NamespaceMap.AddNamespace("resources", new Uri(GlobalVariables.resourcesPrefix));
                g.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> movie in processedResult)
                {
                    var movieTitle = movie.Key.ToString().Split("/").Last();
                    var subject = g.CreateUriNode($"resources:movie/{movieTitle}");
                    triples.Add(new Triple(subject, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Movie")));

                    foreach (KeyValuePair<string, List<string>> property in movie.Value)
                    {
                        var propertyName = property.Key.ToString().Split("/").Last();
                        var valueList = property.Value;
                        if (datasetFields.Contains(propertyName))
                        {
                            AddTriples(subject, propertyName, valueList);
                        }
                    }
                }
            }
            
            
            connector.UpdateGraph(GlobalVariables.resourcesPrefix, triples, null);

        }

        private void AddTriples(IUriNode subject, string propertyName, List<string> valuesList)
        {
            if (propertyParameters.Keys.Contains(propertyName))
            {
                foreach (var value in valuesList)
                {
                    var valueName = value.ToString().Split("/").Last();
                    var node = g.CreateUriNode($@"resources:{valueName}");
                    triples.Add(new Triple(node, g.CreateUriNode("rdf:type"), g.GetUriNode(@$"resources:{string.Concat(propertyName[0].ToString().ToUpper(), propertyName.AsSpan(1))}")));
                    triples.Add(new Triple(node, g.CreateUriNode("resources:name"),g.CreateLiteralNode(valueName)));
                    triples.Add(new Triple(subject, g.CreateUriNode(@$"resources:{propertyParameters[propertyName]}"), node));
                    
                }
            }
            else if (propertyName == "starring")
            {
                foreach (var value in valuesList)
                {
                    var names = value.ToString().Split("/").Last().Replace("_", " ");
                    foreach(var name in names.Split(","))
                    {
                        var decodedName = WebUtility.UrlEncode(name);
                        var node = g.CreateUriNode($@"resources:{decodedName}");
                        triples.Add(new Triple(node, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Actor")));
                        triples.Add(new Triple(node, g.CreateUriNode("resources:name"), g.CreateLiteralNode(decodedName)));
                        triples.Add(new Triple(subject, g.CreateUriNode("resources:starring"), node));
                    }
                    
                }
            }
            else
            {
                if(propertyName == "name") { propertyName = "title"; }//did this so i didn't have to modify the ontology
                foreach (var value in valuesList)
                {
                    var valueName = value.ToString().Split("/").Last();
                    triples.Add(new Triple(subject, g.CreateUriNode(@$"resources:{propertyName}"), g.CreateLiteralNode(valueName)));
                }
            }
        }

        private Dictionary<string, Dictionary<string, List<string>>> ProcessInfoResult(SparqlResultSet results)
        {
            Dictionary<string, Dictionary<string, List<string>>> movieInfo = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (var result in results)
            {
                var movie = result.Value("movie").ToString();
                if (!movieInfo.ContainsKey(movie))
                    movieInfo[movie] = new Dictionary<string, List<string>>();

                movieInfo[movie][result.Value("prop").ToString()] = result.Value("value").ToString().Split("|split|").ToList();

                // movieInfo[result.Value("prop").ToString()] = result.Value("value").ToString().Split("|split|").ToList();              
            }

            return movieInfo;
        }
    }
}
