using RethinkDb.Spec;
using System;
using System.Linq.Expressions;

namespace RethinkDb.QueryTerm
{
    public class UpdateQueryBase<T>
    {
        private readonly ISequenceQuery<T> sequenceTerm;
        private readonly IMutableSingleObjectQuery<T> singleObjectTerm;
        private readonly Expression<Func<T, T>> updateExpression;
        private readonly bool nonAtomic;

        public UpdateQueryBase(ISequenceQuery<T> tableTerm, Expression<Func<T, T>> updateExpression, bool nonAtomic)
        {
            this.sequenceTerm = tableTerm;
            this.updateExpression = updateExpression;
            this.nonAtomic = nonAtomic;
        }

        public UpdateQueryBase(IMutableSingleObjectQuery<T> singleObjectTerm, Expression<Func<T, T>> updateExpression, bool nonAtomic)
        {
            this.singleObjectTerm = singleObjectTerm;
            this.updateExpression = updateExpression;
            this.nonAtomic = nonAtomic;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory, IExpressionConverter expressionConverter)
        {
            var updateTerm = new Term()
            {
                type = Term.TermType.UPDATE,
            };
            if (singleObjectTerm != null)
                updateTerm.args.Add(singleObjectTerm.GenerateTerm(datumConverterFactory, expressionConverter));
            else
                updateTerm.args.Add(sequenceTerm.GenerateTerm(datumConverterFactory, expressionConverter));

            updateTerm.args.Add(ExpressionUtils.CreateFunctionTerm<T, T>(datumConverterFactory, expressionConverter, updateExpression));

            AddOptionalArguments(updateTerm);
            if (nonAtomic)
            {
                updateTerm.optargs.Add(new Term.AssocPair() {
                    key = "non_atomic",
                    val = new Term() {
                        type = Term.TermType.DATUM,
                        datum = new Datum() {
                            type = Datum.DatumType.R_BOOL,
                            r_bool = nonAtomic
                        }
                    }
                });
            }

            return updateTerm;
        }

        protected virtual void AddOptionalArguments(Term updateTerm)
        {
        }
    }
}
