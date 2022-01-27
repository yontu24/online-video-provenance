using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderLibrary.Interfaces
{
    public interface IInsertQueryBuilder
    {
        public IInsertQueryBuilder AddInsertTriple(string triple);
        public IInsertQueryBuilder AddInsertTriple(string predicate, string obj);
        public IInsertQueryBuilder AddInsertTriple(string subject, string prefix, string predicate, string obj);
        public IInsertQueryBuilder AddInsertTripleWithLiteral(string predicate, string obj);
        public IInsertQueryBuilder AddInsertTripleWithLiteral(string subject, string prefix, string predicate, string obj);
        public IInsertQueryBuilder AddInsertTripleWithPrefix(string prefix, string predicate, string obj);
        public IInsertQueryBuilder AddInsertTripleWithPrefixAndLiteral(string prefix, string predicate, string obj);
        public IInsertQueryBuilder AddInsertTripleWithSubject(string subject, string predicate, string obj);
        public IInsertQueryBuilder AddInsertTripleWithSubjectAndLiteral(string subject, string predicate, string obj);
        public IInsertQueryBuilder AddWhereTriple(string triple);
        public IInsertQueryBuilder AddWhereTriple(string predicate, string obj);
        public IInsertQueryBuilder AddWhereTriple(string subject, string prefix, string predicate, string obj);
        public IInsertQueryBuilder AddWhereTripleWithLiteral(string predicate, string obj);
        public IInsertQueryBuilder AddWhereTripleWithLiteral(string subject, string prefix, string predicate, string obj);
        public IInsertQueryBuilder AddWhereTripleWithPrefix(string prefix, string predicate, string obj);
        public IInsertQueryBuilder AddWhereTripleWithPrefixAndLiteral(string prefix, string predicate, string obj);
        public IInsertQueryBuilder AddWhereTripleWithSubject(string subject, string predicate, string obj);
        public IInsertQueryBuilder AddWhereTripleWithSubjectAndLiteral(string subject, string predicate, string obj);
        public IInsertQueryBuilder DeclarePrefix(string prefix, string prefixUri);
        public IInsertQueryBuilder SetSeparator(string separator);
        public IInsertQueryBuilder UseGraphForInsert(string graph);
        public IInsertQueryBuilder UseGraphForWhere(string graph);
        public IInsertQueryBuilder UseSubject(string subject);

        public IInsertQueryBuilder UsePrefix(string prefix);

        public string BuildQuery();
    }
}
