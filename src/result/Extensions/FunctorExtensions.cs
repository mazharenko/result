using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace mazharenko.result;

[PublicAPI]
public static class FunctorExtensions
{
	[MustUseReturnValue]
	public static Result<TOut, TFailure> Map<T, TOut, TFailure>(this Result<T, TFailure> source,
		Func<T, TOut> function)
	{
		return source.Match(
			value => Result<TOut, TFailure>.Success(function(value)!),
			error => Result<TOut, TFailure>.Failure(error!)
		);
	}

	[MustUseReturnValue]
	public static async Task<Result<TOut, TFailure>> MapAsync<T, TOut, TFailure>(this Result<T, TFailure> source,
		Func<T, Task<TOut>> function)
	{
		return await source.MatchAsync(
				   async value => Result<TOut, TFailure>.Success((await function(value).ConfigureAwait(false))!),
				   error => Task.FromResult(Result<TOut, TFailure>.Failure(error!))
			   ).ConfigureAwait(false);
	}

	[MustUseReturnValue]
	public static Result<T, TFailure2> OrMap<T, TFailure, TFailure2>(this Result<T, TFailure> source,
		Func<TFailure, TFailure2> function)
	{
		return source.Match(
			value => Result<T, TFailure2>.Success(value!),
			error => Result<T, TFailure2>.Failure(function(error)!)
		);
	}

	[MustUseReturnValue]
	public static async Task<Result<T, TFailure2>> OrMapAsync<T, TFailure, TFailure2>(this Result<T, TFailure> source,
		Func<TFailure, Task<TFailure2>> function)
	{
		return await source.MatchAsync(
				   value =>  Task.FromResult(Result<T, TFailure2>.Success(value!)),
				   async error => Result<T, TFailure2>.Failure((await function(error).ConfigureAwait(false))!)
			   ).ConfigureAwait(false);
	}
}