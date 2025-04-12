using Hangfire.Server;

namespace Infrastructure.Jobs
{
    /// <summary>
    /// Base class for Hangfire jobs.
    /// </summary>
    public abstract class HangfireJobBase
    {
        protected readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireJobBase"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging.</param>
        public HangfireJobBase(ILogger<HangfireJobBase> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executes the job asynchronously.
        /// </summary>
        /// <param name="context">The context in which the job is performed.</param>
        public async Task ExecuteAsync(PerformContext? context)
        {
            _logger.LogDebug($"Executing job {context?.BackgroundJob}.");
            await ProcessJobAsync();
            _logger.LogDebug($"Finishing job {context?.BackgroundJob}.");
        }

        /// <summary>
        /// Processes the job asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected abstract Task ProcessJobAsync();
    }
}
