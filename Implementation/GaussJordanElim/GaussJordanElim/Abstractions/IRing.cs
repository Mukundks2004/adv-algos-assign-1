namespace GaussJordanElim.Abstractions;

/* Unfortunately I have to use this ugly and broken self referential interface here, 
 * the core problem is that we want a Ring to be indexed by a type T- this is a loose
 * condition and a common requirement when designing an API. However, we have the
 * additional requirement that an IRing added to the same type of IRing produces the
 * same type of IRing. This "sameness" is impossible to encode without self reference.
 * 
 * Even with self reference it is flawed, as an IRing added to the same type of IRing
 * can return a different type of IRing, but this is better than no guarantee of
 * returning an IRing at all.
 */
internal interface IRing<T> : IEquatable<T>, IDeepCloneable<T> where T : IRing<T>
{
	static abstract T operator +(T left, T right);
	static abstract T operator -(T left, T right);
	static abstract T operator *(T left, T right);

	static abstract T Zero { get; }
	static abstract T One { get; }
}
