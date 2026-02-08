using MediatR;
using TextAnalyzerAPI.Application.Models;

namespace TextAnalyzerAPI.Application.Commands;

public record AnalyzeTextCommand(string Content) : IRequest<AnalyzeTextResponse>;
