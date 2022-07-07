using Pyto.Data.Users;
using Pyto.Services.Common;

namespace Pyto.Services.Authentication;

public class RefreshTokenDeleteProcess : ITimedProcess
{
	private readonly IRefreshTokenRepository refreshTokenRepository;
	private readonly ILogger<RefreshTokenDeleteProcess> logger;

	public RefreshTokenDeleteProcess(IRefreshTokenRepository refreshTokenRepository, ILogger<RefreshTokenDeleteProcess> logger)
	{
		this.refreshTokenRepository = refreshTokenRepository;
		this.logger = logger;
	}

	public TimeSpan Period => TimeSpan.FromHours(1);
	public async Task RunAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"Deleting expired refresh tokens.");
		var deletedRows = await refreshTokenRepository.RemoveExpiredTokens(stoppingToken).ConfigureAwait(false);
		logger.LogInformation("Deleted expired tokens: {DeletedRows}", deletedRows.ToString());
	}
}
