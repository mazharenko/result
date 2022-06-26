using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using mazharenko.result.Objects;

namespace mazharenko.result;

[PublicAPI]
public static class Result
{
	public static Result<T, TFailure> From<T, TFailure>(bool successCondition, T success, [DisallowNull] TFailure failure)
	{
		return successCondition ? success : failure;
	}

	public static Result<T, TFailure> From<T, TFailure>(bool successCondition, Func<T> successFactory,
		Func<TFailure> failureFactory)
	{
		return successCondition ? successFactory() : failureFactory()!;
	}

	public static SuccessResult<T> Success<T>(T success) => new(success);

	public static FailureResult<TFailure> Failure<TFailure>([DisallowNull]TFailure failure) => new(failure);
}