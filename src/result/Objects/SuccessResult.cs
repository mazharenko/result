using System.Diagnostics.CodeAnalysis;

namespace mazharenko.result.Objects;

[JetBrains.Annotations.PublicAPI]
public readonly struct SuccessResult<T>
{
	[NotNull]
	public T Value { get; }

	public SuccessResult()
	{
		Value = default!;
	}

	internal SuccessResult([DisallowNull] T value)
	{
		Value = value;
	}

	public Result<T, TFailure> ToResult<TFailure>() => Result<T, TFailure>.Success(Value);
}
