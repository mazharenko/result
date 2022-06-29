using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace mazharenko.result;

[PublicAPI]
public static class Result
{
	[MustUseReturnValue]
	public static Result<T, TFailure> From<T, TFailure>(bool successCondition, [DisallowNull] T success, [DisallowNull] TFailure failure)
	{
		return successCondition ? success : failure;
	}

	[MustUseReturnValue]
	public static Result<T, TFailure> From<T, TFailure>(bool successCondition, Func<T> successFactory,
		Func<TFailure> failureFactory)
	{
		return successCondition ? successFactory()! : failureFactory()!;
	}

	[Pure]
	public static SuccessResult<T> Success<T>([DisallowNull] T success) => new(success);

	[Pure]
	public static FailureResult<TFailure> Failure<TFailure>([DisallowNull]TFailure failure) => new(failure);
}