using GaussJordanElim.Solvers;

namespace SolverApi.Models;

public class SolveRequestDto
{
	public SolveType SolveType { get; set; }

	public double[][] Matrix { get; set; } = [];
}
