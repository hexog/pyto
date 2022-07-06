using System.Security.Cryptography;

namespace Pyto.Models.Extensions;

public static class RandomNumberGeneratorExtensions
{

	public static string GetString(this RandomNumberGenerator rng, int bytesLength)
	{
		Span<byte> bytes = stackalloc byte[bytesLength];
		rng.GetBytes(bytes);
		return Convert.ToBase64String(bytes);
	}
}
