using GaussJordanElim.MatrixEntities;
using GaussJordanElim.Solvers;
using Microsoft.AspNetCore.Mvc;
using SolverApi.Mapping;
using SolverApi.Models;

namespace SolverApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StepsController : ControllerBase
{
    [HttpPost("GetSteps")]
    public ActionResult<List<SolveStepDto>> GetSteps([FromBody] SolveRequestDto request)
    {
        if (request.Matrix is null || request.Matrix.Length == 0 || request.Matrix.Any(row => row is null || row.Length == 0))
        {
            return BadRequest("Matrix must be a non-empty, non-jagged 2D array.");
        }

        try
        {
            var matrix = MatrixMapper.ToRealMatrix(request.Matrix);
            var solver = MatrixSolverFactory.Create<Real>(request.SolveType);
            var (_, steps) = solver.SolveWithSteps(matrix);

            return Ok(MatrixMapper.ToStepDtos(steps));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
