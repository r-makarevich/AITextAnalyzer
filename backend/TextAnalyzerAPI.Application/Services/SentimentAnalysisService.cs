using Microsoft.ML;
using Microsoft.ML.Data;
using TextAnalyzerAPI.Application.Models;
using TextAnalyzerAPI.Domain.Enums;

namespace TextAnalyzerAPI.Application.Services;

public class SentimentAnalysisService : ISentimentAnalysisService
{
    private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

    public SentimentAnalysisService()
    {
        var mlContext = new MLContext();
        
        // Create and train a simple sentiment analysis model
        var emptyData = new List<SentimentData>();
        var data = mlContext.Data.LoadFromEnumerable(emptyData);

        // Define the training pipeline
        var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

        // For initial setup, create a simple rule-based prediction
        // In production, you would train this model with actual data
        _predictionEngine = CreateRuleBasedEngine(mlContext);
    }

    public Sentiment AnalyzeSentiment(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Sentiment.Neutral;
        }

        var input = new SentimentData { Text = text };
        var prediction = _predictionEngine.Predict(input);

        // Simple rule-based sentiment analysis
        var lowerText = text.ToLower();
        
        // Positive keywords
        var positiveWords = new[] { "good", "great", "excellent", "amazing", "wonderful", "fantastic", 
            "love", "best", "awesome", "happy", "nice", "perfect", "beautiful", "brilliant" };
        
        // Negative keywords
        var negativeWords = new[] { "bad", "terrible", "awful", "horrible", "worst", "hate", 
            "poor", "disappointing", "sad", "angry", "useless", "waste", "disgusting" };

        var positiveCount = positiveWords.Count(word => lowerText.Contains(word));
        var negativeCount = negativeWords.Count(word => lowerText.Contains(word));

        if (positiveCount > negativeCount)
        {
            return Sentiment.Positive;
        }
        else if (negativeCount > positiveCount)
        {
            return Sentiment.Negative;
        }
        else
        {
            return Sentiment.Neutral;
        }
    }

    private PredictionEngine<SentimentData, SentimentPrediction> CreateRuleBasedEngine(MLContext mlContext)
    {
        // Create a simple trained model for the prediction engine structure
        var sampleData = new List<SentimentData>
        {
            new SentimentData { Text = "This is great", Label = true },
            new SentimentData { Text = "This is bad", Label = false }
        };

        var trainingData = mlContext.Data.LoadFromEnumerable(sampleData);
        
        var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: nameof(SentimentData.Label), 
                featureColumnName: "Features"));

        var model = pipeline.Fit(trainingData);
        
        return mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
    }
}
