using MediatR;

namespace TextAnalyzerAPI.Application.Commands;

public record AnalyzeTextCommand(string Content) : IRequest<int>;
