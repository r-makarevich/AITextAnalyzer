using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TextAnalyzerAPI.Application.Queries;
using TextAnalyzerAPI.Domain.Entities;
using TextAnalyzerAPI.Domain.Enums;
using TextAnalyzerAPI.Infrastructure.Data;

namespace TextAnalyzerAPI.Application.Tests.Queries;

public class GetAllTextsQueryHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetAllTextsQueryHandler _handler;

    public GetAllTextsQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new GetAllTextsQueryHandler(_context);
    }

    [Fact]
    public async Task Handle_EmptyDatabase_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetAllTextsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithData_ShouldReturnAllRecords()
    {
        // Arrange
        var textAnalyses = new List<TextAnalysis>
        {
            new TextAnalysis
            {
                Id = 1,
                Content = "Test content 1",
                Sentiment = Sentiment.Positive,
                Language = "English",
                CreatedAt = DateTime.UtcNow
            },
            new TextAnalysis
            {
                Id = 2,
                Content = "Test content 2",
                Sentiment = Sentiment.Negative,
                Language = "Spanish",
                CreatedAt = DateTime.UtcNow
            },
            new TextAnalysis
            {
                Id = 3,
                Content = "Test content 3",
                Sentiment = Sentiment.Neutral,
                Language = "French",
                CreatedAt = DateTime.UtcNow
            }
        };

        _context.TextAnalyses.AddRange(textAnalyses);
        await _context.SaveChangesAsync();

        var query = new GetAllTextsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(r => r.Id == 1 && r.Language == "English");
        result.Should().Contain(r => r.Id == 2 && r.Language == "Spanish");
        result.Should().Contain(r => r.Id == 3 && r.Language == "French");
    }

    [Fact]
    public async Task Handle_ShouldMapSentimentCorrectly()
    {
        // Arrange
        var textAnalysis = new TextAnalysis
        {
            Id = 1,
            Content = "Positive test",
            Sentiment = Sentiment.Positive,
            Language = "English",
            CreatedAt = DateTime.UtcNow
        };

        _context.TextAnalyses.Add(textAnalysis);
        await _context.SaveChangesAsync();

        var query = new GetAllTextsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].Sentiment.Should().Be(Sentiment.Positive);
        result[0].SentimentText.Should().Be("Positive");
    }

    [Fact]
    public async Task Handle_ShouldIncludeAllProperties()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;
        var textAnalysis = new TextAnalysis
        {
            Id = 1,
            Content = "Test content",
            Sentiment = Sentiment.Neutral,
            Language = "German",
            CreatedAt = createdAt
        };

        _context.TextAnalyses.Add(textAnalysis);
        await _context.SaveChangesAsync();

        var query = new GetAllTextsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var item = result[0];
        item.Id.Should().Be(1);
        item.Content.Should().Be("Test content");
        item.Sentiment.Should().Be(Sentiment.Neutral);
        item.Language.Should().Be("German");
        item.CreatedAt.Should().BeCloseTo(createdAt, TimeSpan.FromSeconds(1));
    }
}
