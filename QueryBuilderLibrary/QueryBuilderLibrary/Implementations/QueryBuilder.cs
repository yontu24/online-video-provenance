using QueryBuilderLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderLibrary.Implementations
{
    public class QueryBuilder : IQueryBuilder
    {
        private string _declaredPrefixes = string.Empty;
        private string _prefix = string.Empty;
        private string _subject = string.Empty;
        // what's in _subjectAggregated does not need to be in group by
        private string _declaredSubjects = string.Empty;
        private string _aggregatedSubjects = string.Empty;
        private string _groupBy = string.Empty;
        private StringBuilder _whereBody = new StringBuilder();

        public IQueryBuilder DeclarePrefix(string prefix, string prefixUri)
        {
            _declaredPrefixes += $"PREFIX {prefix}:{prefixUri}\r\n";

            return this;
        }

        public IQueryBuilder AddSubject(string subject) => AddMultipleSubjects(new List<string>() { subject });

        public IQueryBuilder AddMultipleSubjects(List<string> subjects)
        {
            var temp = string.Join(" ", subjects.Select(x => "?" + x));
            _declaredSubjects += " " + temp;
            AddGroupBy(subjects);

            return this;
        }

        public IQueryBuilder AddAggregatedSubject(string subject) => AddMultipleAggregatedSubjects(new List<string>() { subject });

        public IQueryBuilder AddMultipleAggregatedSubjects(List<string> subjects)
        {
            var temp = string.Join(" ", subjects.Select(x => $"(GROUP_CONCAT(distinct ?{x}; SEPARATOR = \"|separator|\") as ?{x})"));
            _aggregatedSubjects += " " + temp;

            return this;
        }

        public IQueryBuilder UsePrefix(string prefix)
        {
            _prefix = prefix;

            return this;
        }

        public IQueryBuilder UseSubject(string subject)
        {
            _subject = subject;

            return this;
        }

        public IQueryBuilder AddTriple(string subject, string prefix, string predicate, string obj)
        {
            _whereBody.AppendLine($"{subject} {prefix}:{predicate} ?{obj}");

            return this;
        }

        public IQueryBuilder AddTriple(string predicate, string obj)
        {
            _whereBody.AppendLine($"{_subject} {_prefix}:{predicate} ?{obj}");

            return this;
        }

        public IQueryBuilder AddTripleWithSubject(string subject, string predicate, string obj)
        {
            _whereBody.AppendLine($"{subject} {_prefix}:{predicate} ?{obj}");

            return this;
        }

        public IQueryBuilder AddTripleWithPrefix(string prefix, string predicate, string obj)
        {
            _whereBody.AppendLine($"{_subject} {prefix}:{predicate} ?{obj}");

            return this;
        }

        public IQueryBuilder WithSubjectOfType(string subject, string prefix, string predicate)
        {
            _whereBody.AppendLine($"{subject} a {prefix}:{predicate}");

            return this;
        }

        public IQueryBuilder WithSubjectOfType(string subject, string predicate)
        {
            _whereBody.AppendLine($"{subject} a {_prefix}:{predicate}");

            return this;
        }

        public IQueryBuilder WithSubjectOfType(string predicate)
        {
            _whereBody.AppendLine($"{_subject} a {_prefix}:{predicate}");

            return this;
        }

        public IQueryBuilder AddFilter(string filter)
        {
            _whereBody.AppendLine($"FILTER({filter})");

            return this;
        }

        public IQueryBuilder AddStringFilter(string subject, string value)
        {
            _whereBody.AppendLine($"FILTER( regex(str(?{subject}), \"{value}\") )");

            return this;
        }

        public IQueryBuilder AddStringFilter(string value)
        {
            _whereBody.AppendLine($"FILTER( regex(str(?{_subject}), \"{value}\") )");

            return this;
        }

        public IQueryBuilder AddGroupBy(string subject) => AddGroupBy(new List<string>() { subject });

        public IQueryBuilder AddGroupBy(List<string> subjects)
        {
            var temp = string.Join(" ", subjects.Select(x => "?" + x));
            _groupBy = _groupBy + " " + temp;

            return this;
        }

        public string BuildQuery()
        {
            StringBuilder query = new StringBuilder();
            query
                .AppendLine(_declaredPrefixes)
                .AppendLine($"SELECT {_declaredSubjects} {_aggregatedSubjects} WHERE {{")
                .AppendLine(_whereBody.ToString())
                .AppendLine($"}} GROUP BY {_groupBy}");

            return query.ToString();
        }
    }
}
