using Microsoft.EntityFrameworkCore;

namespace Pyto.Data.Users;

public interface IRefreshTokenRepository
{
	Task AddAsync(string token, Guid userId, DateTime validTo);
	Task<RefreshTokenDbo?> FindValidAsync(string token);

	Task<int> RemoveExpiredTokens(CancellationToken stoppingToken);
	Task RemoveAsync(RefreshTokenDbo refreshToken);
	Task RemoveByUserIdAsync(Guid userId);
}

public class RefreshTokenRepository : IRefreshTokenRepository
{
	private readonly ApplicationDbContext dbContext;

	public RefreshTokenRepository(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	private static DateTime Now => DateTime.UtcNow;

	public Task AddAsync(string token, Guid userId, DateTime validTo)
	{
		var refreshToken = new RefreshTokenDbo
		{
			RefreshToken = token,
			UserId = userId,
			ValidTo = validTo,
			Created = Now,
		};

		dbContext.Add(refreshToken);
		return dbContext.SaveChangesAsync();
	}

	public Task<RefreshTokenDbo?> FindValidAsync(string token)
	{
		var now = Now;

		return dbContext
		   .RefreshTokens
		   .Include(x => x.User)
		   .Where(x => x.RefreshToken == token && x.ValidTo > now)
		   .FirstOrDefaultAsync();
	}

	public async Task<int> RemoveExpiredTokens(CancellationToken stoppingToken)
	{
		var now = Now;

		var tokens = await dbContext.RefreshTokens
		   .Where(x => x.ValidTo < now)
		   .ToListAsync(stoppingToken).ConfigureAwait(false);

		dbContext.RefreshTokens.RemoveRange(tokens);
		await dbContext.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
		return tokens.Count;
	}

	public Task RemoveAsync(RefreshTokenDbo refreshToken)
	{
		dbContext.RefreshTokens.Remove(refreshToken);
		return dbContext.SaveChangesAsync();
	}

	public async Task RemoveByUserIdAsync(Guid userId)
	{
		var tokens = await dbContext.RefreshTokens.Where(x => x.UserId == userId).ToListAsync().ConfigureAwait(false);
		dbContext.RemoveRange(tokens);
		await dbContext.SaveChangesAsync().ConfigureAwait(false);
	}
}
