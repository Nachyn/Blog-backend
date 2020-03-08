using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours
{
    public class RequestPerformanceBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        private readonly Stopwatch _timer;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request
            , CancellationToken cancellationToken
            , RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                _logger.LogWarning(
                    "Long Running Request: ({ElapsedMilliseconds} milliseconds) {@Request}"
                    , elapsedMilliseconds
                    , request);
            }

            return response;
        }
    }
}