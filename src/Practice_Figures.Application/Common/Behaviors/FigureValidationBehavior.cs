using MediatR;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Application.Figures.DTOs;
using Practice_Figures.Application.Figures.Validators;

namespace Practice_Figures.Application.Common.Behaviors;

public class FigureValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly FigureValidator _figureValidator;

    public FigureValidationBehavior(FigureValidator figureValidator)
    {
        _figureValidator = figureValidator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IFigureMutationCommand command || typeof(TResponse) != typeof(FigureCommandResult))
        {
            return await next(cancellationToken);
        }

        var missing = await _figureValidator.ValidateAsync(command, cancellationToken);

        if (missing.Count == 0)
            return await next(cancellationToken);

        var result = FigureCommandResult.ValidationFailed(missing);
        return (TResponse)(object)result;
    }
}
