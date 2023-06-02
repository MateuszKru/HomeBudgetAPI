using System.Diagnostics;

namespace HomeBudgetAPI.Middlewares
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly Stopwatch _stopWatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopWatch = Stopwatch.StartNew();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            if (_stopWatch.ElapsedMilliseconds > 4000)
            {
                _logger.LogWarning($"Request {context.Request.Method} at {context.Request.Path} took {_stopWatch.ElapsedMilliseconds}ms.");
            }
        }
    }
}
