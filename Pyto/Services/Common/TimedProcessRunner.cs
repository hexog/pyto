namespace Pyto.Services.Common;

public interface ITimedProcess
{
	Task RunAsync(CancellationToken stoppingToken);
	TimeSpan Period { get; }
}

public class TimedProcessRunner<TProcess> : BackgroundService
	where TProcess : ITimedProcess
{
	private readonly IServiceProvider serviceProvider;
	private readonly ILogger<TimedProcessRunner<TProcess>> logger;

	public TimedProcessRunner(IServiceProvider serviceProvider, ILogger<TimedProcessRunner<TProcess>> logger)
	{
		this.serviceProvider = serviceProvider;
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		stoppingToken.Register(
			() => logger.LogWarning("Cancellation for process {Process} is called", nameof(TProcess)));

		while (stoppingToken.IsCancellationRequested == false)
		{
			using var services = serviceProvider.CreateScope();
			var process = services.ServiceProvider.GetRequiredService<TProcess>();
			await process.RunAsync(stoppingToken).ConfigureAwait(false);
			await Task.Delay(process.Period, stoppingToken).ConfigureAwait(false);
		}
	}
}
