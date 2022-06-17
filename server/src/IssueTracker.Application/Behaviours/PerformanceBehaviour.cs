using IssueTracker.Application.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IssueTracker.Application.Shared.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
          where TRequest : RequestBase, IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;


        public PerformanceBehaviour( ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();

            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 1000)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogWarning("Laser Long Running Request: " +
                                    "{Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                                    requestName, elapsedMilliseconds, request);
            }

            return response;
        }
    }
}
