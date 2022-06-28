using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace mazharenko.result.Objects;

[PublicAPI]
public readonly struct Result<T, TFailure>
{
	private enum Tag
	{
		Success = 1,
		Failure
	}

	private readonly T? value = default;
	private readonly TFailure failure = default!;
	private readonly Tag tag;

	private Result(T? value)
	{
		tag = Tag.Success;
		this.value = value;
	}

	private Result([DisallowNull] TFailure failure)
	{
		tag = Tag.Failure;
		this.failure = failure ?? throw new ArgumentNullException(nameof(failure));
	}

	public static Result<T, TFailure> Success(T? value) => new(value);

	public static Result<T, TFailure> Failure([DisallowNull] TFailure failure) => new(failure);

	public TResult Match<TResult>(TResult successValue, TResult failureValue)
	{
		return tag switch
		{
			Tag.Success => successValue,
			Tag.Failure => failureValue
		};
	}

	public void Match(Action<T?> successA, Action<TFailure> failureA)
	{
		switch (tag)
		{
			case Tag.Success:
				successA(value);
				break;
			case Tag.Failure:
				failureA(failure);
				break;
		}
	}

	public TResult Match<TResult>(Func<T?, TResult> successF, Func<TFailure, TResult> failureF)
	{
		return tag switch
		{
			Tag.Success => successF(value),
			Tag.Failure => failureF(failure)
		};
	}

	public async Task<TResult> MatchAsync<TResult>(Func<T?, Task<TResult>> successF,
		Func<TFailure, Task<TResult>> failureF)
	{
		return tag switch
		{
			Tag.Success => await successF(value).ConfigureAwait(false),
			Tag.Failure => await failureF(failure).ConfigureAwait(false)
		};
	}

	public async Task MatchAsync(Func<T?, Task> successA, Func<TFailure, Task> failureA)
	{
		switch (tag)
		{
			case Tag.Success:
				await successA(value).ConfigureAwait(false);
				break;
			case Tag.Failure:
				await failureA(failure).ConfigureAwait(false);
				break;
		}
	}

	public static implicit operator Result<T, TFailure>(T from) => Success(from);
	public static implicit operator Result<T, TFailure>([DisallowNull] TFailure from) => Failure(from);

	public static implicit operator Result<T, TFailure>(SuccessResult<T> from) => Success(from.Value);
	public static implicit operator Result<T, TFailure>(FailureResult<TFailure> from) => Failure(from.Value);

	public override string ToString()
	{
		return Match(ok => $"SUCCESS({ok})", e => $"FAILURE({e})");
	}
}