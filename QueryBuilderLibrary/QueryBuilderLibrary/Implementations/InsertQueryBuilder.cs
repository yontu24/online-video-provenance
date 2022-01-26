﻿using QueryBuilderLibrary.Interfaces;
using System;
using System.Text;

namespace QueryBuilderLibrary.Implementations
{
    class InsertQueryBuilder : IInsertQueryBuilder
    {
        #region fields

        private StringBuilder _declaredPrefixes = new StringBuilder();
        private string _prefix = string.Empty;
        private string _subject = string.Empty;
        private StringBuilder _insertBody = new StringBuilder();
        private string _insertGraph = string.Empty;
        private string _subjectQuestionMark = string.Empty;
        private StringBuilder _whereBody = new StringBuilder();
        private string _whereGraph = string.Empty;

        private string _separator = "|separator|";

        #endregion

        #region properties

        public string Separator
        {
            get => _separator;
            set
            {
                _separator = value;
            }
        }


        public IInsertQueryBuilder SetSeparator(string separator)
        {
            _separator = separator;

            return this;
        }

        #endregion

        public IInsertQueryBuilder DeclarePrefix(string prefix, string prefixUri)
        {
            _declaredPrefixes.AppendLine($"PREFIX {prefix}:<{prefixUri}>");
            _prefix = prefix;

            return this;
        }

        public IInsertQueryBuilder UseGraphForInsert(string graph)
        {
            _insertGraph = graph;
            InsertGraphChanged();

            return this;
        }

        public IInsertQueryBuilder UseSubject(string subject)
        {
            _subject = subject;

            return this;
        }

        public IInsertQueryBuilder AddInsertTriple(string triple)
        {
            _insertBody.AppendLine(triple);

            return this;
        }

        public IInsertQueryBuilder AddInsertTriple(string subject, string prefix, string predicate, string obj)
        {
            CheckSubject(subject, obj);

            _insertBody.AppendLine($"{_subjectQuestionMark}{subject} {prefix}:{predicate} ?{obj} .");

            return this;
        }

        public IInsertQueryBuilder AddInsertTriple(string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_subject) || string.IsNullOrEmpty(_prefix))
                throw new Exception("Subject and/or Prefix have not been set");

            CheckSubject(_subject, _prefix);


            _insertBody.AppendLine($"{_subjectQuestionMark}{_subject} {_prefix}:{predicate} ?{obj} .");

            return this;
        }

        public IInsertQueryBuilder AddInsertTripleWithSubject(string subject, string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_prefix))
                throw new Exception("Prefix has not been set");

            CheckSubject(subject, _prefix);

            _insertBody.AppendLine($"{_subjectQuestionMark}{subject} {_prefix}:{predicate} ?{obj} .");

            return this;
        }

        public IInsertQueryBuilder AddInsertTripleWithPrefix(string prefix, string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_prefix))
                throw new Exception("Subject has not been set");

            CheckSubject(_subject, prefix);

            _insertBody.AppendLine($"{_subjectQuestionMark}{_subject} {prefix}:{predicate} ?{obj} .");

            return this;
        }

        public IInsertQueryBuilder AddInsertTripleWithLiteral(string subject, string prefix, string predicate, string obj)
        {
            CheckSubject(subject, obj);

            _insertBody.AppendLine($"{_subjectQuestionMark}{subject} {prefix}:{predicate} {obj} .");

            return this;
        }

        public IInsertQueryBuilder AddInsertTripleWithLiteral(string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_subject) || string.IsNullOrEmpty(_prefix))
                throw new Exception("Subject and/or Prefix have not been set");

            CheckSubject(_subject, _prefix);


            _insertBody.AppendLine($"{_subjectQuestionMark}{_subject} {_prefix}:{predicate} {obj} .");

            return this;
        }

        public IInsertQueryBuilder AddInsertTripleWithSubjectAndLiteral(string subject, string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_prefix))
                throw new Exception("Prefix has not been set");

            CheckSubject(subject, _prefix);

            _insertBody.AppendLine($"{_subjectQuestionMark}{subject} {_prefix}:{predicate} {obj} .");

            return this;
        }

        public IInsertQueryBuilder AddInsertTripleWithPrefixAndLiteral(string prefix, string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_prefix))
                throw new Exception("Subject has not been set");

            CheckSubject(_subject, prefix);

            _insertBody.AppendLine($"{_subjectQuestionMark}{_subject} {prefix}:{predicate} {obj} .");

            return this;
        }

        public IInsertQueryBuilder UseGraphForWhere(string graph)
        {
            _whereGraph = graph;
            WhereGraphChanged();

            return this;
        }

        public IInsertQueryBuilder AddWhereTriple(string triple)
        {
            _whereBody.AppendLine(triple);

            return this;
        }

        public IInsertQueryBuilder AddWhereTriple(string subject, string prefix, string predicate, string obj)
        {
            CheckSubject(subject, obj);

            _whereBody.AppendLine($"?{subject} {prefix}:{predicate} ?{obj} .");

            return this;
        }

        public IInsertQueryBuilder AddWhereTriple(string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_subject) || string.IsNullOrEmpty(_prefix))
                throw new Exception("Subject and/or Prefix have not been set");

            CheckSubject(_subject, _prefix);


            _whereBody.AppendLine($"?{_subject} {_prefix}:{predicate} ?{obj} .");

            return this;
        }

        public IInsertQueryBuilder AddWhereTripleWithSubject(string subject, string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_prefix))
                throw new Exception("Prefix has not been set");

            CheckSubject(subject, _prefix);

            _whereBody.AppendLine($"?{subject} {_prefix}:{predicate} ?{obj} .");

            return this;
        }

        public IInsertQueryBuilder AddWhereTripleWithPrefix(string prefix, string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_prefix))
                throw new Exception("Subject has not been set");

            CheckSubject(_subject, prefix);

            _whereBody.AppendLine($"?{_subject} {prefix}:{predicate} ?{obj} .");

            return this;
        }

        public IInsertQueryBuilder AddWhereTripleWithLiteral(string subject, string prefix, string predicate, string obj)
        {
            CheckSubject(subject, obj);

            _whereBody.AppendLine($"?{subject} {prefix}:{predicate} {obj} .");

            return this;
        }

        public IInsertQueryBuilder AddWhereTripleWithLiteral(string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_subject) || string.IsNullOrEmpty(_prefix))
                throw new Exception("Subject and/or Prefix have not been set");

            CheckSubject(_subject, _prefix);


            _whereBody.AppendLine($"?{_subject} {_prefix}:{predicate} {obj} .");

            return this;
        }

        public IInsertQueryBuilder AddWhereTripleWithSubjectAndLiteral(string subject, string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_prefix))
                throw new Exception("Prefix has not been set");

            CheckSubject(subject, _prefix);

            _whereBody.AppendLine($"?{subject} {_prefix}:{predicate} {obj} .");

            return this;
        }

        public IInsertQueryBuilder AddWhereTripleWithPrefixAndLiteral(string prefix, string predicate, string obj)
        {
            if (string.IsNullOrEmpty(_prefix))
                throw new Exception("Subject has not been set");

            CheckSubject(_subject, prefix);

            _whereBody.AppendLine($"?{_subject} {prefix}:{predicate} {obj} .");

            return this;
        }

        public string BuildQuery()
        {
            StringBuilder query = new StringBuilder();

            query
                .AppendLine(_declaredPrefixes.ToString())
                .AppendLine("INSERT {")
                .AppendLine(_insertBody.ToString())
                .AppendLine("}")
                .AppendLine("WHERE {")
                .AppendLine(_whereBody.ToString())
                .AppendLine("}");

            return query.ToString();
        }

        private void InsertGraphChanged()
        {
            if (!string.IsNullOrEmpty(_insertBody.ToString()))
                _insertBody.AppendLine("}");

            if (!_insertBody.ToString().Contains("GRAPH "))
                _insertBody.Insert(0, $"GRAPH {_insertGraph } {{");
            else
                _insertBody.AppendLine($"GRAPH {_insertGraph} {{");
        }

        private void WhereGraphChanged()
        {
            if (!string.IsNullOrEmpty(_insertBody.ToString()))
                _whereBody.AppendLine("}");

            if (!_whereBody.ToString().Contains("GRAPH "))
                _whereBody.Insert(0, $"GRAPH {_whereGraph} {{");
            else
                _whereBody.AppendLine($"GRAPH {_whereGraph} {{");
        }

        private void CheckSubject(string subject, string obj)
        {
            if (subject.Contains("http") || subject.Contains("/"))
                _subjectQuestionMark = "?";
            else
                _subjectQuestionMark = string.Empty;
        }
    }
}
