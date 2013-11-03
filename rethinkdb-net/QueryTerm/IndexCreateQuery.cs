using RethinkDb.Spec;
using System.Linq.Expressions;
using System;

namespace RethinkDb.QueryTerm
{
    public class IndexCreateQuery<TTable, TIndexExpression> : IWriteQuery<DmlResponse>
    {
        private readonly ITableQuery<TTable> tableTerm;
        private readonly string indexName;
        private readonly Expression<Func<TTable, TIndexExpression>> indexExpression;

        public IndexCreateQuery(ITableQuery<TTable> tableTerm, string indexName, Expression<Func<TTable, TIndexExpression>> indexExpression)
        {
            this.tableTerm = tableTerm;
            this.indexName = indexName;
            this.indexExpression = indexExpression;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory, IExpressionConverter expressionConverter)
        {
            var indexCreate = new Term()
            {
                type = Term.TermType.INDEX_CREATE,
            };
            indexCreate.args.Add(tableTerm.GenerateTerm(datumConverterFactory, expressionConverter));
            indexCreate.args.Add(new Term() {
                type = Term.TermType.DATUM,
                datum = new Datum() {
                    type = Datum.DatumType.R_STR,
                    r_str = indexName
                },
            });
            indexCreate.args.Add(ExpressionUtils.CreateFunctionTerm(datumConverterFactory, indexExpression));
            return indexCreate;
        }
    }
}

