using Microsoft.AspNetCore.Mvc;

namespace SolverApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StepsController : ControllerBase
{
    [HttpGet("GetSteps")]
    public ActionResult<string[][]> GetSteps()
    {
        var steps = new string[][]
        {
            new[] { "Step 1", "Build coefficient matrix" },
            new[] { "Step 2", "Reduce to row echelon form" },
            new[] { "Step 3", "Back substitute to solve" },
        };

        return Ok(steps);
    }
}
