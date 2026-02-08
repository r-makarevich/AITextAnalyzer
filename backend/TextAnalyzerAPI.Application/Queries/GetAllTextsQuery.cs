using MediatR;
using TextAnalyzerAPI.Domain.Entities;

namespace TextAnalyzerAPI.Application.Queries;

public record GetAllTextsQuery : IRequest<List<TextAnalysis>>;
