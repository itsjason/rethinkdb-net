using RethinkDb.Spec;
using System;
using System.Linq.Expressions;

namespace RethinkDb.QueryTerm
{
    public class DistinctQuery<T> : ISequenceQuery<T>
    {
        private readonly ISequenceQuery<T> sequenceQuery;

        public DistinctQuery(ISequenceQuery<T> sequenceQuery)
        {
            this.sequenceQuery = sequenceQuery;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory, IExpressionConverter expressionConverter)
        {
            var term = new Term()
            {
                type = Term.TermType.DISTINCT,
            };
            term.args.Add(sequenceQuery.GenerateTerm(datumConverterFactory, expressionConverter));
            return term;
        }
    }
}