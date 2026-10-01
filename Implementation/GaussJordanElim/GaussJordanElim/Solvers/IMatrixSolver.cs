using GaussJordanElim.MatrixEntities;

namespace GaussJordanElim.Solvers;

internal interface IMatrixSolver<T> where T : IMatrixEntry<T>, new()
{
	T[,] Solve(T[,] matrix);

	(T[,] Result, List<SolverStep<T>> Steps) SolveWithSteps(T[,] matrix)
	{
		var result = Solve(matrix);
		return (result, [new SolverStep<T>(result, "Solved (no step-by-step trace available for this solver)")]);
	}
}
