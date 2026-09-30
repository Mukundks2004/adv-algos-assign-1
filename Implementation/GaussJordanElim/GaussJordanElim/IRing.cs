namespace GaussJordanElim;

/* Unfortunately I have to use this ugly and broken self referential interface here, 
 * the core problem is that we want a Ring to be indexed by a type T- this is a loose
 * condition and a common requirement when designing an API. However, we have the
 * additional requirement that an IRing added to the same type of IRing produces the
 * same type of IRing. This "sameness" is impossible to encode without self reference.
 */
internal interface IRing<T> where T : IRing<T>
{
	static abstract T operator +(T left, T right);
	static abstract T operator -(T left, T right);
	static abstract T operator *(T left, T right);
}
