using System;
using System.Threading;
using System.Threading.Tasks;

namespace RethinkDb
{
    public interface IConnection : IDisposable
    {
        IDatumConverterFactory DatumConverterFactory
        {
            get;
            set;
        }

        IExpressionConverter ExpressionConverter
        {
            get;
            set;
        }

        ILogger Logger
        {
            get;
            set;
        }

        TimeSpan QueryTimeout
        {
            get;
            set;
        }

        #region Asynchronous API; synchronous API is provided by extension methods

        Task<T> RunAsync<T>(IDatumConverterFactory datumConverterFactory, IExpressionConverter expressionConverter, IScalarQuery<T> queryObject, CancellationToken cancellationToken);

        IAsyncEnumerator<T> RunAsync<T>(IDatumConverterFactory datumConverterFactory, IExpressionConverter expressionConverter, ISequenceQuery<T> queryObject);

        #endregion
    }
}

