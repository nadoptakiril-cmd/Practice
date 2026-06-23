using MediatR;
using Practice_Figures.Core.Interfaces;

namespace Practice_Figures.Application.Figures.Commands;

public record DeleteFigureCommand(int Id) : IRequest;

public class DeleteFigureCommandHandler : IRequestHandler<DeleteFigureCommand>
{
    private readonly IFigureRepository _figureRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFigureCommandHandler(IFigureRepository figureRepository, IUnitOfWork unitOfWork)
    {
        _figureRepository = figureRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteFigureCommand request, CancellationToken cancellationToken)
    {
        var figure = await _figureRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (figure is null)
            return;

        figure.IsDeleted = true;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
