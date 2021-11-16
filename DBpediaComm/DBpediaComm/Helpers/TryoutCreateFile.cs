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
        public static void CreateFile(Dictionary<string, List<string>> movieGenres)
        {
            /*Graph g = new Graph();

            IUriNode film = g.CreateUriNode(UriFactory.Create()
            IUriNode hasGenre = g.CreateUriNode(UriFactory.Create("http://example.org/hasGenre"));
            foreach (string title in movieGenres.Keys)
            {
                IUriNode myRdf = g.CreateUriNode(UriFactory.Create(title));
                foreach (string genre in movieGenres[title])
                {
                    ILiteralNode genreNode = g.CreateLiteralNode(genre);
                    g.Assert(new Triple(myRdf, hasGenre, genreNode));
                }
            }*/

            OntologyGraph g = new OntologyGraph();
            var filmClass = g.CreateOntologyClass(UriFactory.Create("http://dbpedia.org/ontology/Film"));
            var genreClass = g.CreateOntologyClass(UriFactory.Create("http://dbpedia.org/ontology/genre"));
            var a = g.CreateUriNode("rdf:type");
            var prop = g.CreateUriNode("rdf:Property");
            
            foreach (string title in movieGenres.Keys)
            {
                var film = g.CreateOntologyClass(UriFactory.Create(title));
                film.AddType(filmClass);
                // g.Assert(new Triple(film, a, filmClass));
                foreach (string genreStr in movieGenres[title])
                {
                    var genre = g.CreateOntologyClass(UriFactory.Create(genreStr));
                    genre.AddType(genreClass);

                    g.Assert(new Triple(film.Resource, genreClass.Resource, genre.Resource));
                    // film.AddResourceProperty(, genre.Resource, true);
                    // film.AddLiteralProperty(UriFactory.Create(genreStr), g.CreateLiteralNode(genreStr), false);
                    // film.AddResourceProperty(genreStr, g.CreateUriNode(genreStr), true);
                    // genreClass.AddLiteralProperty(UriFactory.Create(genreStr), g.CreateLiteralNode(genreStr), true);
                }
            }

            RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
            rdfxmlwriter.Save(g, "HelloWorld.rdf");
        }

        public static void QueryFile()
        {
            IGraph g = new Graph();
            // RdfXmlParser parser = new RdfXmlParser();
            // parser.Load(g, "HelloWorld.rdf");

            /*            //Load using a Filename
                        FileLoader.Load(g, "HelloWorld.rdf");
                        // dbopedia.org/resource
                        //  ?movie <http://dbpedia.org/ontology/genre> ?genre .
                        SparqlQueryParser qParser = new SparqlQueryParser();
                        SparqlQuery query = qParser.ParseFromString(@"

                            SELECT * WHERE {
                                    ?movie a <http://dbpedia.org/resource/Albedo_One> .

                                } LIMIT 100");

                        var results = g.ExecuteQuery(query);*/

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
