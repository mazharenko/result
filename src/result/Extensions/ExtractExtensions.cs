using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace mazharenko.result;

[PublicAPI]
public static class ExtractExtensions
{
	[Pure]
	internal static (bool isSuccess, T value, TFailure failure) Get<T, TFailure>(Result<T, TFailure> result)
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

	[MustUseReturnValue]
	public static T Or<T, TFailure>(this Result<T, TFailure> source, Func<TFailure, T> function)
	{
		return source.Match(value => value, function);
	}

	[MustUseReturnValue]
	public static T Or<T, TFailure>(this Result<T, TFailure> source, Func<T> function)
	{
		return source.Or(_ => function());
	}

	[Pure]
	public static T Or<T, TFailure>(this Result<T, TFailure> source, T fallbackValue)
	{
		return source.Or(_ => fallbackValue);
	}
	
	[MustUseReturnValue]
	public static async Task<T> OrAsync<T, TFailure>(this Result<T, TFailure> source, Func<TFailure, Task<T>> function)
	{
		return await source.MatchAsync(Task.FromResult, function).ConfigureAwait(false);
	}

	[MustUseReturnValue]
	public static Task<T> OrAsync<T, TFailure>(this Result<T, TFailure> source, Func<Task<T>> function)
	{
		return source.OrAsync(_ => function());
	}
	
	[Pure]
	public static (ICollection<T> oks, ICollection<TFailure> failures) Partition<T, TFailure>(
		this IEnumerable<Result<T, TFailure>> results)
	{
		var failures = new List<TFailure>();
		var oks = results.ChooseSuccess(failures.Add);
		return (oks, failures);
	}
	
	public static ICollection<T> ChooseSuccess<T, TFailure>(this IEnumerable<Result<T, TFailure>> source,
		Action<TFailure> actionForFailures)
	{
		return source.Choose(x => x, actionForFailures);
	}

	public static ICollection<T> Choose<TIn, T, TFailure>(this IEnumerable<TIn> source,
		Func<TIn, Result<T, TFailure>> f,
		Action<TFailure> actionForFailures)
	{
		return source.Select(f)
			.Select(Get)
			.Where(t => t.isSuccess)
			.Select(t => t.value)
			.ToList();
	}
}