using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Datasets;
using VDS.RDF.Writing;

namespace DBpediaComm.Helpers
{
    public class TryoutCreateFile
    {
        public static void CreateFile(Dictionary<string, Dictionary<string, List<string>>> movieGenres)
        {
            OntologyGraph g = new OntologyGraph();
            var filmClass = g.CreateOntologyClass(UriFactory.Create("http://WADe-ovi.org/ontology/Film"));
            
            Dictionary<string, OntologyClass> oClasses = new Dictionary<string, OntologyClass>();
            foreach(string filmProp in movieGenres[movieGenres.Keys.First()].Keys)
            {
                oClasses[filmProp] = g.CreateOntologyClass(UriFactory.Create($"http://WADe-ovi.org/resource/{filmProp}"));
            }

            // var genreClass = g.CreateOntologyClass(UriFactory.Create("http://WADe-ovi.org/ontology/Genre"));
            var a = g.CreateUriNode("rdf:type");
            // var prop = g.CreateUriNode("rdf:Property");

            foreach (string titleUri in movieGenres.Keys)
            {
                string title = titleUri.Split('/').Last();
                var film = g.CreateOntologyClass(UriFactory.Create($"http://WADe-ovi.org/page/{title}"));
                film.AddType(filmClass);

                foreach(string propUri in movieGenres[titleUri].Keys)
                {
                    string filmProp = propUri.Split('/').Last();

                    if(filmProp == "budget")
                    {
                        continue;
                    }

                    foreach (string valUri in movieGenres[titleUri][propUri])
                    {
                        string val = valUri.Split('/').Last();
                        string graphUri = $"http://WADe-ovi.org/resource/{val}";
                        
                        var valClass = g.Nodes.Where(x => 
                        x.ToString() == graphUri
                        ).FirstOrDefault();// g.GetUriNode(graphUri);

                        if (valClass == null)
                        {
                            valClass = g.CreateUriNode(UriFactory.Create(graphUri));
                            g.Assert(new Triple(valClass, a, oClasses[propUri].Resource));
                        }

                        if (!g.ContainsTriple(new Triple(film.Resource, oClasses[propUri].Resource, valClass)))
                            g.Assert(new Triple(film.Resource, oClasses[propUri].Resource, valClass));
                    }
                }

                /*foreach (string genreStr in movieGenres[title])
                {
                    var genre = g.CreateOntologyClass(UriFactory.Create(genreStr));
                    genre.AddType(genreClass);

                    g.Assert(new Triple(film.Resource, genreClass.Resource, genre.Resource));
                    // film.AddResourceProperty(, genre.Resource, true);
                    // film.AddLiteralProperty(UriFactory.Create(genreStr), g.CreateLiteralNode(genreStr), false);
                    // film.AddResourceProperty(genreStr, g.CreateUriNode(genreStr), true);
                    // genreClass.AddLiteralProperty(UriFactory.Create(genreStr), g.CreateLiteralNode(genreStr), true);
                }*/
            }

            RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
            rdfxmlwriter.Save(g, "HelloWorld.rdf");
        }

        public static void QueryFile()
        {
            IGraph g = new Graph();

            g.LoadFromFile("HelloWorld.rdf");
            ISparqlDataset ds = new InMemoryDataset(g);
            LeviathanQueryProcessor processor = new LeviathanQueryProcessor(ds);

            SparqlQueryParser qParser = new SparqlQueryParser();
            SparqlQuery query = qParser.ParseFromString(@"

                SELECT * WHERE {
                        ?movie a <http://dbpedia.org/ontology/Film> .
                        ?movie <http://dbpedia.org/ontology/genre> ?genre      
                    } LIMIT 100");

            var results = processor.ProcessQuery(query);
        }

    }
}
