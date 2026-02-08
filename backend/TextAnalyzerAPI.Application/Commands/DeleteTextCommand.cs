using MediatR;

namespace TextAnalyzerAPI.Application.Commands;

public record DeleteTextCommand(int Id) : IRequest<bool>;
