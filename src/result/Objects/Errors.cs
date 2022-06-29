using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace mazharenko.result;

[JetBrains.Annotations.PublicAPI]
public sealed class Errors<T> : IFormattable
{
	internal Errors([DisallowNull] T root) : this(root, Array.Empty<Errors<T>>())
	{
	}

	internal Errors([DisallowNull]T root, IEnumerable<Errors<T>> innerFailures)
		: this(root, new ReadOnlyCollection<Errors<T>>(innerFailures.ToList()))
	{
	}

	internal Errors([DisallowNull]T root, IReadOnlyCollection<Errors<T>> innerFailures)
	{
		Root = root;
		InnerFailures = innerFailures;
	}

	[NotNull]
	public T Root { get; }
	public IReadOnlyCollection<Errors<T>> InnerFailures { get; }

	[Pure]
	public override string ToString()
	{
		return ToString(null, null);
	}

	[Pure]
	public string ToString(string? format, IFormatProvider? provider)
	{
		if (string.IsNullOrEmpty(format)) format = "G";
		provider ??= CultureInfo.CurrentCulture;

		var match = Regex.Match(format, "^G(\\d{0,})$");
		if (!match.Success)
			throw new FormatException($"The {format} format string is not supported");

		var indentCountString = match.Groups[1].Value;
		int.TryParse(indentCountString, out var indent);

		var sb = new StringBuilder();
		var writer = new StringWriter(sb);

		const int indentLength = 2;
		writer.Write(new string(' ', indent * indentLength));
		writer.Write(Root);

		var indentString = new string(' ', (indent + 1) * indentLength);
		foreach (var inner in InnerFailures)
		{
			writer.WriteLine();
			writer.Write(indentString);
			writer.Write("└─> ");
			var innerString = inner.ToString($"G{indent + 1}", provider);
			writer.Write(innerString[(indentLength * (indent + 1))..]);
		}

		return sb.ToString();
	}
}