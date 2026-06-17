using MediatR;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Application.Figures.DTOs;

namespace Practice_Figures.Application.Figures.Queries;

public record GetFiguresQuery : IRequest<List<FigureResponseDto>>;

public class GetFiguresQueryHandler : IRequestHandler<GetFiguresQuery, List<FigureResponseDto>>
{
    private readonly IFigureRepository _figureRepository;

    public GetFiguresQueryHandler(IFigureRepository figureRepository)
    {
        _figureRepository = figureRepository;
    }

    public async Task<List<FigureResponseDto>> Handle(GetFiguresQuery request, CancellationToken cancellationToken)
    {
        var figures = await _figureRepository.GetAllWithDetailsAsync(cancellationToken);

        return figures
            .Select(figure => figure.ToResponseDto())
            .ToList();
    }
}
