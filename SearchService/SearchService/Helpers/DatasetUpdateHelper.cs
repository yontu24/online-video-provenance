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
        public static void UpdateDatasetMovies(Dictionary<string, Dictionary<string, List<string>>> processedResult)
        {
            SesameHttpProtocolConnector connector = new SesameHttpProtocolConnector("http://localhost:8080/rdf4j-server", "wade1") { Timeout = int.MaxValue };
            connector.LoadGraph(g,new Uri("http://www.wade-ovi.org/resources#"));
            g.NamespaceMap.AddNamespace("resources", CommonVariables.ResourcesPrefixUri);
            g.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> movie in processedResult)
            {
                var movieTitle = movie.Key.ToString().Split("/").Last();
                var subject = g.CreateUriNode($"resources:movie/{movieTitle}");
                triples.Add(new Triple( subject, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Movie")));

                foreach (KeyValuePair<string, List<string>> property in movie.Value)
                {
                    var propertyName = property.Key.ToString().Split("/").Last();
                    var valueList = property.Value;
                    if (CommonVariables.datasetFields.Contains(propertyName))
                    {
                        AddTriplesMovie(subject, propertyName, valueList);
                    }
                }
            }

            connector.UpdateGraph(CommonVariables.ResourcesPrefixUri, triples, null);
        }

        public static void UpdateDatasetPersons(Dictionary<string, Dictionary<string, List<string>>> processedResult)
        {
            SesameHttpProtocolConnector connector = new SesameHttpProtocolConnector("http://localhost:8080/rdf4j-server", "wade1") { Timeout = int.MaxValue };
            connector.LoadGraph(g, new Uri("http://www.wade-ovi.org/resources#"));
            g.NamespaceMap.AddNamespace("resources", CommonVariables.ResourcesPrefixUri);
            g.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> movie in processedResult)
            {
                var personName = movie.Key.ToString().Split("/").Last();
                var subject = g.CreateUriNode($"resources:{personName}");
                triples.Add(new Triple(subject, g.CreateUriNode("rdf:type"), g.GetUriNode("resources:Person")));

                foreach (KeyValuePair<string, List<string>> property in movie.Value)
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
        private static void AddTriplesMovie(IUriNode subject, string propertyName, List<string> valuesList)
        {
            if (CommonVariables.propertyParameters.Keys.Contains(propertyName))
            {
                foreach (var value in valuesList)
                {
                    var valueName = value.ToString().Split("/").Last();
                    var node = g.CreateUriNode($@"resources:{valueName}");
                    triples.Add(new Triple(node, g.CreateUriNode("rdf:type"), g.GetUriNode(@$"resources:{string.Concat(propertyName[0].ToString().ToUpper(), propertyName.AsSpan(1))}")));
                    triples.Add(new Triple(node, g.CreateUriNode("resources:name"), g.CreateLiteralNode(valueName)));
                    triples.Add(new Triple(subject, g.CreateUriNode(@$"resources:{CommonVariables.propertyParameters[propertyName]}"), node));

                }
            }
            else if (propertyName == "starring")
            {
                foreach (var value in valuesList)
                {
                    var names = value.ToString().Split("/").Last().Replace("_", " ");
                    foreach (var name in names.Split(","))
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
                if (propertyName == "name") { propertyName = "title"; }//did this so i didn't have to modify the ontology
                foreach (var value in valuesList)
                {
                    var valueName = value.ToString().Split("/").Last();
                    triples.Add(new Triple(subject, g.CreateUriNode(@$"resources:{propertyName}"), g.CreateLiteralNode(valueName)));
                }
            }
        }

        private static void AddTriplesPerson(IUriNode subject, string propertyName, List<string> valuesList)
        {
            foreach (var value in valuesList)
            {
                var valueName = value.ToString().Split("/").Last();
                triples.Add(new Triple(subject, g.CreateUriNode(@$"resources:{propertyName}"), g.CreateLiteralNode(valueName)));
            }

        }
    }
}
