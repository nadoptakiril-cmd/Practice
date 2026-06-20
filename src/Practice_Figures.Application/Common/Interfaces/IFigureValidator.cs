namespace Practice_Figures.Application.Common.Interfaces;

public interface IFigureValidator
{
    Task<List<string>> ValidateAsync(
        IFigureMutationCommand command,
        CancellationToken cancellationToken);
}
