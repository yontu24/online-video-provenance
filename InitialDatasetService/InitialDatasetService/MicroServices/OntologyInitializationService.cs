using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;

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
            INode someClass = g.GetUriNode(new Uri("http://www.wade-ovi.org/Movie"));

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
        }
    }
}
