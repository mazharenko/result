using JetBrains.Annotations;

namespace mazharenko.result.Objects;

[PublicAPI]
public readonly struct SuccessResult<T>
{
	public T? Value { get; }

	public SuccessResult()
	{
		Value = default;
	}

	internal SuccessResult(T? value)
	{
		Value = value;
	}

	public Result<T, TFailure> ToResult<TFailure>() => Result<T, TFailure>.Success(Value);
}
