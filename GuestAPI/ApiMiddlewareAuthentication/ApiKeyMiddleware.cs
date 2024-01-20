namespace GuestAPI.ApiMiddlewareAuthentication
{
    /// <summary>
    /// Middleware for API key authentication.
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="configuration">The configuration to retrieve the API key.</param>
        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _apiKey = configuration["ApiKey:Key"]; // Replace with your configuration key for API key
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to perform API key authentication.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!context.Request.Headers.TryGetValue("ApiKey", out var apiKey) || apiKey != _apiKey)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Invalid API key.");

                    // Logging unauthorized access attempt
                    _logger.LogWarning("Unauthorized access attempt. Invalid API key.");

                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, "An error occurred in API key middleware.");
                context.Response.StatusCode = 500; // Internal Server Error
                await context.Response.WriteAsync("An unexpected error occurred.");

                // Logging the unexpected error
                _logger.LogError("An unexpected error occurred in API key middleware.", ex);
            }
        }
    }
}
