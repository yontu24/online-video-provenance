using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderLibrary.Interfaces
{
    public interface IQueryBuilder
    {
        public IQueryBuilder AddAggregatedSubject(string subject);
        public IQueryBuilder AddFilter(string filter);
        public IQueryBuilder AddGroupBy(List<string> subjects);
        public IQueryBuilder AddGroupBy(string subject);
        public IQueryBuilder AddMultipleAggregatedSubjects(List<string> subjects);
        public IQueryBuilder AddMultipleSubjects(List<string> subjects);
        public IQueryBuilder AddMultipleDistinctSubjects(List<string> subjects);
        public IQueryBuilder AddStringFilter(string value);
        public IQueryBuilder AddStringFilter(string subject, string value);
        public IQueryBuilder AddSubject(string subject);
        public IQueryBuilder AddDistinctSubject(string subject);
        public IQueryBuilder AddTriple(string predicate, string obj);
        public IQueryBuilder AddTriple(string subject, string prefix, string predicate, string obj);
        public IQueryBuilder AddTripleWithPrefix(string prefix, string predicate, string obj);
        public IQueryBuilder AddTripleWithSubject(string subject, string predicate, string obj);
        public string BuildQuery(bool noGroupBy);
        public IQueryBuilder DeclarePrefix(string prefix, string prefixUri);
        public IQueryBuilder UsePrefix(string prefix);
        public IQueryBuilder AddLimit(uint limit);
        public IQueryBuilder UseSubject(string subject);
        public IQueryBuilder WithSubjectOfType(string predicate);
        public IQueryBuilder WithSubjectOfType(string subject, string predicate);
        public IQueryBuilder WithSubjectOfType(string subject, string prefix, string predicate);
        public IQueryBuilder AddMultipleValuesFilter(string subject, List<string> values, bool literalValuesFlag = false)
;
    }
}
