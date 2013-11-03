using RethinkDb.Spec;
using System;
using System.Linq.Expressions;

namespace RethinkDb.QueryTerm
{
    public class MapQuery<TOriginal, TTarget> : ISequenceQuery<TTarget>
    {
        private readonly ISequenceQuery<TOriginal> sequenceQuery;
        private readonly Expression<Func<TOriginal, TTarget>> mapExpression;

        public MapQuery(ISequenceQuery<TOriginal> sequenceQuery, Expression<Func<TOriginal, TTarget>> mapExpression)
        {
            this.sequenceQuery = sequenceQuery;
            this.mapExpression = mapExpression;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory, IExpressionConverter expressionConverter)
        {
            var mapTerm = new Term()
            {
                type = Term.TermType.MAP,
            };
            mapTerm.args.Add(sequenceQuery.GenerateTerm(datumConverterFactory, expressionConverter));
            mapTerm.args.Add(ExpressionUtils.CreateFunctionTerm<TOriginal, TTarget>(datumConverterFactory, mapExpression));
            return mapTerm;
        }
    }
}
