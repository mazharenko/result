using JetBrains.Annotations;

namespace System;

[PublicAPI]
public static class Extensions
{
	public static T DisplayPipe<T>(this T o)
	{
		o.Display();
		return o;
	}
}