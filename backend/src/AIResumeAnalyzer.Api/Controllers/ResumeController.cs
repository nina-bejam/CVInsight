using AIResumeAnalyzer.Application.DTOs;
using AIResumeAnalyzer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIResumeAnalyzer.Api.Controllers;

[ApiController]
[Route("api/resume")]
[Produces("application/json")]
public sealed class ResumeController(IResumeAnalyzerService analyzerService) : ControllerBase
{
    /// <summary>
    /// Analyzes resume text against a target role using the local rule-based engine.
    /// </summary>
    [HttpPost("analyze")]
    [ProducesResponseType(typeof(ResumeAnalysisResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ResumeAnalysisResponseDto>> Analyze(
        [FromBody] AnalyzeResumeRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await analyzerService.AnalyzeAsync(request, cancellationToken);
        return Ok(result);
    }
}
