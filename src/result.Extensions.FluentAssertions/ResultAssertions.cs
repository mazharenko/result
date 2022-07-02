using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using JetBrains.Annotations;

namespace mazharenko.result;

[PublicAPI]
public class ResultAssertions<T, TFailure> : ReferenceTypeAssertions<Result<T, TFailure>, ResultAssertions<T, TFailure>>
{
	public ResultAssertions(Result<T, TFailure> subject) : base(subject)
	{
	}

	protected override string Identifier => "result";

	[CustomAssertion]
	public AndWhichConstraint<ResultAssertions<T, TFailure>, T> BeSuccess(string because = "",
		params object[] becauseArgs)
	{
		Execute.Assertion
			.ForCondition(Subject.Match(true, false))
			.BecauseOf(because, becauseArgs)
			.FailWith(
				"Expected {context:result} to be in a successful state{reason}, but found {0}.", Subject);

		return Subject.Match
		(
			success =>
				new AndWhichConstraint<ResultAssertions<T, TFailure>, T>(this, success),
			_ => 
				/* unlikely to get there unless custom assertion scopes are used */
				new AndWhichConstraint<ResultAssertions<T, TFailure>, T>(this, default(T)!)
		);
	}

	[CustomAssertion]
	public AndWhichConstraint<ResultAssertions<T, TFailure>, TFailure> BeFailure(string because = "",
		params object[] becauseArgs)
	{
		Execute.Assertion
			.ForCondition(Subject.Match(false, true))
			.BecauseOf(because, becauseArgs)
			.FailWith(
				"Expected {context:result} to be in a failed state{reason}, but found {0}.", Subject);

		return Subject.Match
		(
			_ =>
				/* unlikely to get there unless custom assertion scopes are used */
				new AndWhichConstraint<ResultAssertions<T, TFailure>, TFailure>(this, default(TFailure)!),
			failure => new AndWhichConstraint<ResultAssertions<T, TFailure>, TFailure>(this, failure)
		);
	}
}