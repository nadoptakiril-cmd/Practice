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
        try
        {
            var command = request.ToCreateCommand();
            var figure = await _mediator.Send(command);

            return Ok(figure);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FigureMutationRequest request)
    {
        try
        {
            var command = request.ToUpdateCommand(id);
            var updated = await _mediator.Send(command);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteFigureCommand(id));

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
