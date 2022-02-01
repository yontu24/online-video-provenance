using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Storage;

namespace SearchService.Helpers
{
    public static class DatasetUpdateHelper
    {
        private static IGraph g = new Graph();
        private static List<Triple> triples = new();
        private static string  separator = "|separator|";
        public static void UpdateDatasetMovies(Dictionary<string, Dictionary<string, Dictionary<string, string>>> processedResult)
        {
            SesameHttpProtocolConnector connector = new SesameHttpProtocolConnector("http://localhost:8080/rdf4j-server", "wade1") { Timeout = int.MaxValue };
            connector.LoadGraph(g,new Uri("http://www.wade-ovi.org/resources#"));
            g.NamespaceMap.AddNamespace("resources", CommonVariables.ResourcesPrefixUri);
            g.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> movie in processedResult)
            {
                var movieTitle = WebUtility.UrlEncode(movie.Key.ToString().Split("/").Last().Replace("_"," "));
                var subject = g.CreateUriNode($"resources:movie/{movieTitle}");
                triples.Add(new Triple( subject, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Movie")));

                foreach (var property in movie.Value)
                {
                    
                    
                        var propertyName = property.Key.ToString().Split("/").Last().Replace("_", " ");
                        var valueList = property.Value;
                        if (CommonVariables.datasetFields.Contains(propertyName))
                        {
                            AddTriplesMovie(subject, propertyName, valueList, property.Key);
                        }
                    
                    
                }
            }

            connector.UpdateGraph(CommonVariables.ResourcesPrefixUri, triples, null);
        }

        public static void UpdateDatasetPersons(Dictionary<string, Dictionary<string, Dictionary<string, string>>> processedResult)
        {
            SesameHttpProtocolConnector connector = new SesameHttpProtocolConnector("http://localhost:8080/rdf4j-server", "wade1") { Timeout = int.MaxValue };
            connector.LoadGraph(g, new Uri("http://www.wade-ovi.org/resources#"));
            g.NamespaceMap.AddNamespace("resources", CommonVariables.ResourcesPrefixUri);
            g.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> movie in processedResult)
            {
                var personName = WebUtility.UrlEncode(movie.Key.ToString().Split("/").Last().Replace("_", " "));
                var subject = g.CreateUriNode($"resources:{personName}");
                triples.Add(new Triple(subject, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Person")));

                foreach (var  property in movie.Value)
                {
                    var propertyName = property.Key.ToString().Split("/").Last();
                    var valueList = property.Value;
                    if (CommonVariables.personAttributes.Contains(propertyName))
                    {
                        AddTriplesPerson(subject, propertyName, valueList);
                    }
                }
            }

            connector.UpdateGraph(CommonVariables.ResourcesPrefixUri, triples, null);
        }
        private static void AddTriplesMovie(IUriNode subject, string propertyName, Dictionary<string , string> valuesList, string propertyUri)
        {
            if (CommonVariables.propertyParameters.Keys.Contains(propertyName))
            {
                foreach (var value in valuesList)
                {
                    var valueName = WebUtility.UrlEncode(value.Value);
                    var node = g.CreateUriNode($@"resources:{valueName}");
                    triples.Add(new Triple(node, g.CreateUriNode("rdf:type"), g.GetUriNode(@$"resources:{string.Concat(propertyName[0].ToString().ToUpper(), propertyName.AsSpan(1))}")));
                    triples.Add(new Triple(node, g.CreateUriNode("resources:name"), g.CreateLiteralNode(valueName)));
                    triples.Add(new Triple(subject, g.CreateUriNode(@$"resources:{CommonVariables.propertyParameters[propertyName]}"), node));
                    triples.Add(new Triple(node, g.CreateUriNode("resources:dbpediaReference"), g.CreateLiteralNode(value.Key)));

                }
            }
            else if (propertyName == "starring")
            {
                foreach (var value in valuesList)
                {
                    var name = value.Value;
                    var encodedName = WebUtility.UrlEncode(name);
                    var node = g.CreateUriNode($@"resources:{encodedName}");
                    triples.Add(new Triple(node, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Actor")));
                    triples.Add(new Triple(node, g.CreateUriNode("resources:name"), g.CreateLiteralNode(encodedName)));
                    triples.Add(new Triple(subject, g.CreateUriNode("resources:starring"), node));
                    triples.Add(new Triple(node, g.CreateUriNode("resources:dbpediaReference"), g.CreateLiteralNode(value.Key)));

                }
            }
            else
            {
                if (propertyName == "name") { propertyName = "title"; }//did this so i didn't have to modify the ontology
                foreach (var value in valuesList)
                {
                    var valueName = WebUtility.UrlEncode(value.Value);
                    triples.Add(new Triple(subject, g.CreateUriNode(@$"resources:{propertyName}"), g.CreateLiteralNode(valueName)));
                }
            }
        }

        private static void AddTriplesPerson(IUriNode subject, string propertyName, Dictionary<string, string> valuesList)
        {
            foreach (var value in valuesList)
            {
                var valueName = WebUtility.UrlEncode(value.Value);
                triples.Add(new Triple(subject, g.CreateUriNode(@$"resources:{propertyName}"), g.CreateLiteralNode(valueName)));
            }

        }
    }
}
