using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TextAnalyzerAPI.Application.Commands;
using TextAnalyzerAPI.Application.Interfaces;
using TextAnalyzerAPI.Domain.Entities;
using TextAnalyzerAPI.Domain.Enums;
using TextAnalyzerAPI.Infrastructure.Data;

namespace TextAnalyzerAPI.Application.Tests.Commands;

public class DeleteTextCommandHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly DeleteTextCommandHandler _handler;

    public DeleteTextCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new DeleteTextCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_ExistingId_ShouldReturnTrueAndDeleteRecord()
    {
        // Arrange
        var textAnalysis = new TextAnalysis
        {
            Id = 1,
            Content = "Test content",
            Sentiment = Sentiment.Positive,
            Language = "English",
            CreatedAt = DateTime.UtcNow
        };

        _context.TextAnalyses.Add(textAnalysis);
        await _context.SaveChangesAsync();

        var command = new DeleteTextCommand(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deletedEntity = await _context.TextAnalyses.FindAsync(1);
        deletedEntity.Should().BeNull();
    }

    [Fact]
    public async Task Handle_NonExistingId_ShouldReturnFalse()
    {
        // Arrange
        var command = new DeleteTextCommand(999);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_DeletedRecord_ShouldNotAffectOtherRecords()
    {
        // Arrange
        var textAnalysis1 = new TextAnalysis
        {
            Id = 1,
            Content = "Test content 1",
            Sentiment = Sentiment.Positive,
            Language = "English",
            CreatedAt = DateTime.UtcNow
        };

        var textAnalysis2 = new TextAnalysis
        {
            Id = 2,
            Content = "Test content 2",
            Sentiment = Sentiment.Negative,
            Language = "Spanish",
            CreatedAt = DateTime.UtcNow
        };

        _context.TextAnalyses.AddRange(textAnalysis1, textAnalysis2);
        await _context.SaveChangesAsync();

        var command = new DeleteTextCommand(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var remainingRecords = await _context.TextAnalyses.ToListAsync();
        remainingRecords.Should().HaveCount(1);
        remainingRecords[0].Id.Should().Be(2);
    }
}
