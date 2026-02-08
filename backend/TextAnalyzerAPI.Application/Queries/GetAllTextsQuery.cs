using MediatR;
using TextAnalyzerAPI.Application.Models;

namespace TextAnalyzerAPI.Application.Queries;

public record GetAllTextsQuery : IRequest<List<AnalyzeTextResponse>>;
