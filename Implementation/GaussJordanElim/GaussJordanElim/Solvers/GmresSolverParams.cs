namespace GaussJordanElim.Solvers;

internal readonly struct GmresSolverParams
{
	readonly double initialGuess;
	readonly double tolerance;
	readonly double maxIterations;
}
