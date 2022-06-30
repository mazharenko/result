using System;
using System.Linq;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace mazharenko.result;

[PublicAPI]
public static class ZipExtensions
{
	public static Result<T, Errors<TFailure>> ToErrors<T, TFailure>(this Result<T, TFailure> source)
	{
		return source.OrMap(f => f!.ToErrors());
	}
	
	public static IEnumerable<Result<T, Errors<TFailure>>> ToErrors<T, TFailure>(
		this IEnumerable<Result<T, TFailure>> results) 
		=> results.Select(ToErrors);
	
	public static Result<T, Errors<TFailure>> Zip<T, TFailure>(this Result<T, Errors<TFailure>> source, 
		Func<Errors<TFailure>, TFailure> zipRootFactory)
	{
		return source.OrMap(f => zipRootFactory(f)!.ToErrors(f));
	}
	
	public static Result<(T1, T2), Errors<TFailure>> Zip<T1, T2, TFailure>(
		this (Result<T1, Errors<TFailure>>, Result<T2, Errors<TFailure>>) source,
		Func<ICollection<Errors<TFailure>>, TFailure> zipRootFactory)
	{
		var r1 = source.Item1.Get();
		var r2 = source.Item2.Get();

		if (r1.isSuccess && r2.isSuccess)
			return (r1.value, r2.value);
		
		var failures = new List<Errors<TFailure>>();

		if (!r1.isSuccess)
			failures.Add(r1.failure);
		if (!r2.isSuccess)
			failures.Add(r2.failure);

		return zipRootFactory(failures)!.ToErrors(failures);
	}
	
	public static Result<(T1, T2, T3), Errors<TFailure>> Zip<T1, T2, T3, TFailure>(
		this (Result<T1, Errors<TFailure>>, Result<T2, Errors<TFailure>>, Result<T3, Errors<TFailure>>) source,
		Func<ICollection<Errors<TFailure>>, TFailure> zipRootFactory)
	{
		var r1 = source.Item1.Get();
		var r2 = source.Item2.Get();
		var r3 = source.Item3.Get();

		if (r1.isSuccess && r2.isSuccess && r3.isSuccess)
			return (r1.value, r2.value, r3.value);
		
		var failures = new List<Errors<TFailure>>();

		if (!r1.isSuccess)
			failures.Add(r1.failure);
		if (!r2.isSuccess)
			failures.Add(r2.failure);
		if (!r3.isSuccess)
			failures.Add(r3.failure);

		return zipRootFactory(failures)!.ToErrors(failures);
	}
	
	public static Result<(T1, T2, T3, T4), Errors<TFailure>> Zip<T1, T2, T3, T4, TFailure>(
		this (Result<T1, Errors<TFailure>>, Result<T2, Errors<TFailure>>, Result<T3, Errors<TFailure>>, Result<T4, Errors<TFailure>>) source,
		Func<ICollection<Errors<TFailure>>, TFailure> zipRootFactory)
	{
		var r1 = source.Item1.Get();
		var r2 = source.Item2.Get();
		var r3 = source.Item3.Get();
		var r4 = source.Item4.Get();

		if (r1.isSuccess && r2.isSuccess && r3.isSuccess && r4.isSuccess)
			return (r1.value, r2.value, r3.value, r4.value);
		
		var failures = new List<Errors<TFailure>>();

		if (!r1.isSuccess)
			failures.Add(r1.failure);
		if (!r2.isSuccess)
			failures.Add(r2.failure);
		if (!r3.isSuccess)
			failures.Add(r3.failure);
		if (!r4.isSuccess)
			failures.Add(r4.failure);

		return zipRootFactory(failures)!.ToErrors(failures);
	}
}