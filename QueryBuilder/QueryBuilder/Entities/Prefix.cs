using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryBuilderService.Entities
{ 
    public static class Prefix
    {
        // declare whatever prefixes we're gonna use similar to this one
        public static readonly string Ovi = "ovi";

        public static readonly string Movie = "movie";
        public static readonly string Title = "title";
        public static readonly string Genre = "genre";

        // people related prefixes
        public static readonly string Person = "person";
        public static readonly string Actor = "actor";
        public static readonly string Director = "director";
        public static readonly string Crew = "crew";

        public static List<string> GetAll()
        {
            List<string> props = new List<string>();
            Type t = typeof(Prefix);
            
            // select not working on GetFields()
            foreach (var prop in t.GetFields())
                props.Add(prop.GetValue(t).ToString());

            return props;
        }
    }
}
