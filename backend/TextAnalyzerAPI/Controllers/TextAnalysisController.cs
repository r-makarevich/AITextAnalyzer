using MediatR;
using Microsoft.AspNetCore.Mvc;
using TextAnalyzerAPI.Application.Commands;
using TextAnalyzerAPI.Application.Models;
using TextAnalyzerAPI.Application.Queries;

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
    public async Task<ActionResult<AnalyzeTextResponse>> Analyze([FromBody] string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return BadRequest("Content cannot be empty.");
        }

        AnalyzeTextCommand command = new AnalyzeTextCommand(content);
        AnalyzeTextResponse result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AnalyzeTextResponse>>> GetAll()
    {
        GetAllTextsQuery query = new GetAllTextsQuery();
        List<AnalyzeTextResponse> result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        DeleteTextCommand command = new DeleteTextCommand(id);
        bool result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound($"Text analysis with ID {id} not found.");
        }

        return Ok(new { message = "Text analysis deleted successfully.", id });
    }
}
