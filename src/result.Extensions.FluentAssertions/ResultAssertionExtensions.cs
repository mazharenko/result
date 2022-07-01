using JetBrains.Annotations;

namespace mazharenko.result;

[PublicAPI]
public static class ResultAssertionExtensions
{
	[Pure]
	public static ResultAssertions<T, TFailure> Should<T, TFailure>(this Result<T, TFailure> result)
	{
		return new ResultAssertions<T, TFailure>(result);
	}
}