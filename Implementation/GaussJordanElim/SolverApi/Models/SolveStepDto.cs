namespace SolverApi.Models;

public class SolveStepDto
{
	public double[][] State { get; set; } = [];

	public string Description { get; set; } = string.Empty;
}
