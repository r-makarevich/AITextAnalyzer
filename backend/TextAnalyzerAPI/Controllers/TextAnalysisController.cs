using MediatR;
using Microsoft.AspNetCore.Mvc;
using TextAnalyzerAPI.Application.Commands;
using TextAnalyzerAPI.Application.Queries;
using TextAnalyzerAPI.Domain.Entities;

namespace TextAnalyzerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TextAnalysisController : ControllerBase
{
    private readonly IMediator _mediator;

    public TextAnalysisController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<int>> Analyze([FromBody] string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return BadRequest("Content cannot be empty.");
        }

        var command = new AnalyzeTextCommand(content);
        var id = await _mediator.Send(command);

        return Ok(id);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<TextAnalysis>>> GetAll()
    {
        var query = new GetAllTextsQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
