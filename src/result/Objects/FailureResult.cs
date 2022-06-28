using System;
using System.Diagnostics.CodeAnalysis;

namespace mazharenko.result;

[JetBrains.Annotations.PublicAPI]
public readonly struct FailureResult<T>
{
	[NotNull]
	public T Value { get; }

	internal FailureResult([DisallowNull] T value)
	{
		Value = value ?? throw new ArgumentNullException(nameof(value));
	}

	public Result<TSuccess, T> ToResult<TSuccess>() => Result<TSuccess, T>.Failure(Value);
}