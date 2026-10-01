namespace GaussJordanElim.Solvers;

internal sealed class SolverStep<T>(T[,] state, string description)
{
	public T[,] State { get; } = state;

	public string Description { get; } = description;
}
