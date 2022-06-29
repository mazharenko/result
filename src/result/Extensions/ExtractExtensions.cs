using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace mazharenko.result;

[PublicAPI]
public static class ExtractExtensions
{
	[Pure]
	private static (bool, T, TFailure) Get<T, TFailure>(Result<T, TFailure> result)
	{
		return result.Match(v => (true, v, default(TFailure)!), f => (false, default(T)!, f));
	}

	[Pure]
	public static bool TryGet<T, TFailure>(this Result<T, TFailure> result,
		[MaybeNullWhen(false)] out T value) 
		where T : class
	{
		(var isSuccess, value, _) = Get(result);
		return isSuccess;
	}

	[Pure]
	public static bool TryGet<T, TFailure>(this Result<T, TFailure> result,
		[MaybeNullWhen(false)] out T value,
		[MaybeNullWhen(true)] out TFailure failure)
		where T : class
		where TFailure : class
	{
		(var isSuccess, value, failure) = Get(result);
		return isSuccess;
	}

	[Pure]
	public static bool TryGetFailure<T, TFailure>(this Result<T, TFailure> result,
		[MaybeNullWhen(false)] out TFailure failure)
		where TFailure : class
	{
		(var isSuccess, _, failure) = Get(result);
		return isSuccess;
	}
}