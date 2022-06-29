using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Formatting;
using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace mazharenko.result.Interactive;

internal static class ResultExtensions
{
	public static string FormatResult<T, TFailure>(this Result<T, TFailure> result)
	{
		var typeHash = result.Match(typeof(T).GetHashCode(), typeof(TFailure).GetHashCode());
		return (
				   (span[style: $"border: 4px solid {result.Match("green", "red")}; " +
								$"background-color: hsla({typeHash % 360}, 100%, 60%, 0.2); " +
								"padding: 10px; display: inline-block"](
						   result.Match<object?>(value => value, error => error)
					   ))
			   ).ToString();
	}
}

[UsedImplicitly]
public class ResultKernelExtension : IKernelExtension
{
	public Task OnLoadAsync(Kernel kernel)
	{
		Formatter.Register(
			type: typeof(Result<,>),
			formatter: (obj, writer) =>
			{
				writer.WriteLine(ResultExtensions.FormatResult((dynamic)obj));
			}, HtmlFormatter.MimeType);

		return Task.CompletedTask;
	}
}