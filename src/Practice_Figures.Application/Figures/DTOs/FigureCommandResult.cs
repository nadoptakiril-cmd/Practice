namespace Practice_Figures.Application.Figures.DTOs;

public enum FigureCommandStatus
{
    Success,
    NotFound,
    ValidationFailed
}

public class FigureCommandResult
{
    public FigureCommandStatus Status { get; init; }
    public FigureResponseDto? Figure { get; init; }
    public List<string> Errors { get; init; } = new();

    public static FigureCommandResult Success(FigureResponseDto figure) =>
        new()
        {
            Status = FigureCommandStatus.Success,
            Figure = figure
        };

    public static FigureCommandResult NotFound() =>
        new()
        {
            Status = FigureCommandStatus.NotFound
        };

    public static FigureCommandResult ValidationFailed(List<string> errors) =>
        new()
        {
            Status = FigureCommandStatus.ValidationFailed,
            Errors = errors
        };
}
