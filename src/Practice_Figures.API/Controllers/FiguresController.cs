using MediatR;
using Microsoft.AspNetCore.Mvc;
using Practice_Figures.API.Contracts;
using Practice_Figures.Application.Figures.Commands;
using Practice_Figures.Application.Figures.DTOs;
using Practice_Figures.Application.Figures.Queries;

namespace Practice_Figures.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FiguresController : ControllerBase
{
    private readonly IMediator _mediator;

    public FiguresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FigureResponseDto>>> GetAll()
    {
        var figures = await _mediator.Send(new GetFiguresQuery());

        return Ok(figures);
    }

    [HttpPost]
    public async Task<ActionResult<FigureResponseDto>> Create(FigureMutationRequest request)
    {
        var command = request.ToCreateCommand();
        var result = await _mediator.Send(command);

        if (result.Status == FigureCommandStatus.ValidationFailed)
        {
            return BadRequest($"Not found: {string.Join("; ", result.Errors)}");
        }

        return Ok(result.Figure);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<FigureResponseDto>> Update(int id, FigureMutationRequest request)
    {
        var command = request.ToUpdateCommand(id);
        var result = await _mediator.Send(command);

        if (result.Status == FigureCommandStatus.NotFound)
            return NotFound();

        if (result.Status == FigureCommandStatus.ValidationFailed)
        {
            return BadRequest($"Not found: {string.Join("; ", result.Errors)}");
        }

        return Ok(result.Figure);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteFigureCommand(id));

        return NoContent();
    }
}
