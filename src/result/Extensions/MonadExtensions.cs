using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace mazharenko.result;

[PublicAPI]
public static class MonadExtensions
{
	[MustUseReturnValue]
	public static Result<TOut, TFailure> Bind<T, TOut, TFailure>(this Result<T, TFailure> source,
		Func<T, Result<TOut, TFailure>> function)
	{
		return source.Match(
			function,
			error => Result<TOut, TFailure>.Failure(error!)
		);
	}
	
	public static Result<T3, TFailure> SelectMany<T1, T2, T3, TFailure>(this Result<T1, TFailure> source,
		Func<T1, Result<T2, TFailure>> function,
		Func<T1, T2, T3> projection)
	{
		return source.Bind(el => function(el).Map(el2 => projection(el, el2)));
	}

	[MustUseReturnValue]
	public static async Task<Result<TOut, TFailure>> BindAsync<T, TOut, TFailure>(this Result<T, TFailure> source,
		Func<T, Task<Result<TOut, TFailure>>> function)
	{
		return await source.MatchAsync(
			function,
			error => Task.FromResult(Result<TOut, TFailure>.Failure(error!))
		).ConfigureAwait(false);
	}

	[MustUseReturnValue]
	public static Result<T, TFailure2> OrBind<T, TFailure, TFailure2>(this Result<T, TFailure> source,
		Func<TFailure, Result<T, TFailure2>> function)
	{
		return source.Match(
			value => Result<T, TFailure2>.Success(value!),
			function
		);
	}

	[MustUseReturnValue]
	public static async Task<Result<T, TFailure2>> OrBindAsync<T, TFailure, TFailure2>(this Result<T, TFailure> source,
		Func<TFailure, Task<Result<T, TFailure2>>> function)
	{
		return await source.MatchAsync(
				   value => Task.FromResult(Result<T, TFailure2>.Success(value!)),
				   function
			   ).ConfigureAwait(false);
	}
	
	
}