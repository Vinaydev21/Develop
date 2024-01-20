using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GuestAPI.Authentication
{
    /// <summary>
    /// Authentication handler for API key authentication.
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The authentication options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="encoder">The URL encoder.</param>
        /// <param name="clock">The system clock.</param>
        /// <param name="configuration">The configuration to retrieve the API key.</param>
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Handles the authentication process for API key.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Request.Headers.TryGetValue("ApiKey", out var apiKeyHeader))
                {
                    return Task.FromResult(AuthenticateResult.Fail("API key not provided."));
                }

                var apiKey = apiKeyHeader.FirstOrDefault();
                var configuredApiKey = _configuration["ApiKey:Key"];

                // Replace this with your actual API key validation logic
                if (apiKey == configuredApiKey)
                {
                    var identity = new ClaimsIdentity("ApiKey");
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }

                Logger.LogWarning("Invalid API key provided.");
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
            }
            catch (Exception ex)
            {                
                Logger.LogError(ex, "An error occurred during API key authentication.");
                return Task.FromResult(AuthenticateResult.Fail("An unexpected error occurred."));
            }
        }
    }
}