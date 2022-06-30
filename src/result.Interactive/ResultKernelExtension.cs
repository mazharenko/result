using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Formatting;
using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace mazharenko.result.Interactive;


[UsedImplicitly]
public class ResultKernelExtension : IKernelExtension
{
	private static PocketView FormatResult<T, TFailure>(Result<T, TFailure> result)
	{
		var typeHash = result.Match(typeof(T).GetHashCode(), typeof(TFailure).GetHashCode());
		return (
				   (span[style: $"border: 4px solid {result.Match("green", "red")}; " +
								$"background-color: hsla({typeHash % 360}, 100%, 60%, 0.2); " +
								"white-space: pre-line;" +
								"padding: 10px; display: inline-block"](
						   (result.Match<object?>(value => value, error => error))
					   ))
			   );
	}

	private static PocketView FormatErrors<T>(IEnumerable<Errors<T>> errors)
	{
		return ul[style: "padding-left: 20pt; list-style-type: none"](
			errors.Select(error => 
				li(
					span(error.Root),
					FormatErrors(error.InnerFailures)
				)
			)
		);
	}
	
	private static PocketView FormatErrors<T>(Errors<T> errors)
	{
		return FormatErrors(new[] { errors });
	}
	
	public Task OnLoadAsync(Kernel kernel)
	{
		Formatter.Register(
			type: typeof(Result<,>),
			formatter: (obj, writer) =>
			{
				writer.WriteLine(FormatResult((dynamic)obj).ToString());
			}, HtmlFormatter.MimeType);

		Formatter.Register(
			type: typeof(Errors<>),
			formatter: (obj, writer) =>
			{
				writer.WriteLine(FormatErrors((dynamic)obj).ToString());
			}, HtmlFormatter.MimeType);

		return Task.CompletedTask;
	}
}