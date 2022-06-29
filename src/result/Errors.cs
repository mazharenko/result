using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace mazharenko.result;

[PublicAPI]
public static class Errors
{
	public static Errors<T> Create<T>([DisallowNull] T root)
		=> new(root);

	public static Errors<T> ToErrors<T>([DisallowNull] this T root) 
		=> Create(root);

	public static Errors<T> Create<T>([DisallowNull] T root, params Errors<T>[] innerFailures)
		=> new(root, (ICollection<Errors<T>>)innerFailures);
	
	public static Errors<T> ToErrors<T>([DisallowNull] this T root, params Errors<T>[] innerFailures)
		=> Create(root, innerFailures);

	public static Errors<T> Create<T>([DisallowNull] T root, IEnumerable<Errors<T>> innerFailures) 
		=> new(root, innerFailures);
	
	public static Errors<T> ToErrors<T>([DisallowNull] this T root, IEnumerable<Errors<T>> innerFailures) 
		=> Create(root, innerFailures);
}