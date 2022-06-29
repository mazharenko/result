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
		var failures = new List<Errors<TFailure>>();

		Result<T, Errors<TFailure>> AddFailure<T>(Errors<TFailure> failure)
		{
			failures.Add(failure);
			return failure;
		}

		var r1 = source.Item1.OrBind(AddFailure<T1>);
		var r2 = source.Item2.OrBind(AddFailure<T2>);

		var zippedOk =
			from value1 in r1
			from value2 in r2
			select (value1, value2);

		return zippedOk.OrMap(_ => zipRootFactory(failures)!.ToErrors(failures));
	}
	
	public static Result<(T1, T2, T3), Errors<TFailure>> Zip<T1, T2, T3, TFailure>(
		this (Result<T1, Errors<TFailure>>, Result<T2, Errors<TFailure>>, Result<T3, Errors<TFailure>>) source,
		Func<ICollection<Errors<TFailure>>, TFailure> zipRootFactory)
	{
		var failures = new List<Errors<TFailure>>();

		Result<T, Errors<TFailure>> AddFailure<T>(Errors<TFailure> failure)
		{
			failures.Add(failure);
			return failure;
		}

		var r1 = source.Item1.OrBind(AddFailure<T1>);
		var r2 = source.Item2.OrBind(AddFailure<T2>);
		var r3 = source.Item3.OrBind(AddFailure<T3>);

		var zippedOk =
			from value1 in r1
			from value2 in r2
			from value3 in r3
			select (value1, value2, value3);

		return zippedOk.OrMap(_ => zipRootFactory(failures)!.ToErrors(failures));
	}
	
	public static Result<(T1, T2, T3, T4), Errors<TFailure>> Zip<T1, T2, T3, T4, TFailure>(
		this (Result<T1, Errors<TFailure>>, Result<T2, Errors<TFailure>>, Result<T3, Errors<TFailure>>, Result<T4, Errors<TFailure>>) source,
		Func<ICollection<Errors<TFailure>>, TFailure> zipRootFactory)
	{
		var failures = new List<Errors<TFailure>>();

		Result<T, Errors<TFailure>> AddFailure<T>(Errors<TFailure> failure)
		{
			failures.Add(failure);
			return failure;
		}

		var r1 = source.Item1.OrBind(AddFailure<T1>);
		var r2 = source.Item2.OrBind(AddFailure<T2>);
		var r3 = source.Item3.OrBind(AddFailure<T3>);
		var r4 = source.Item4.OrBind(AddFailure<T4>);

		var zippedOk =
			from value1 in r1
			from value2 in r2
			from value3 in r3
			from value4 in r4
			select (value1, value2, value3, value4);

		return zippedOk.OrMap(_ => zipRootFactory(failures)!.ToErrors(failures));
	}
}