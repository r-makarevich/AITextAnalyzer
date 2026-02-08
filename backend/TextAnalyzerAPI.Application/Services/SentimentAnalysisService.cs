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
        var mlContext = new MLContext(seed: 0);
        
        // Create training data with sentiment examples
        var trainingData = new List<SentimentData>
        {
            new SentimentData { Text = "This is great", Label = true },
            new SentimentData { Text = "I love this", Label = true },
            new SentimentData { Text = "Excellent work", Label = true },
            new SentimentData { Text = "Amazing and wonderful", Label = true },
            new SentimentData { Text = "Fantastic experience", Label = true },
            new SentimentData { Text = "Best thing ever", Label = true },
            new SentimentData { Text = "Awesome and perfect", Label = true },
            new SentimentData { Text = "Beautiful and brilliant", Label = true },
            new SentimentData { Text = "Good and happy", Label = true },
            new SentimentData { Text = "Nice and pleasant", Label = true },
            new SentimentData { Text = "This is bad", Label = false },
            new SentimentData { Text = "I hate this", Label = false },
            new SentimentData { Text = "Terrible quality", Label = false },
            new SentimentData { Text = "Awful and horrible", Label = false },
            new SentimentData { Text = "Worst experience", Label = false },
            new SentimentData { Text = "Poor and disappointing", Label = false },
            new SentimentData { Text = "Sad and angry", Label = false },
            new SentimentData { Text = "Useless waste", Label = false },
            new SentimentData { Text = "Disgusting product", Label = false },
            new SentimentData { Text = "Very unhappy", Label = false }
        };

        var data = mlContext.Data.LoadFromEnumerable(trainingData);

        // Define the training pipeline
        var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: nameof(SentimentData.Label), 
                featureColumnName: "Features"));

        // Train the model
        var model = pipeline.Fit(data);
        
        // Create prediction engine
        _predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
    }

    public Sentiment AnalyzeSentiment(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Sentiment.Neutral;
        }

        var input = new SentimentData { Text = text };
        var prediction = _predictionEngine.Predict(input);

        // Use ML.NET prediction result to determine sentiment
        // prediction.Prediction is true for positive sentiment, false for negative
        // prediction.Probability represents confidence in the prediction
        // Use a threshold to determine if sentiment is strong enough, otherwise return neutral
        const float confidenceThreshold = 0.6f;
        
        if (prediction.Probability >= confidenceThreshold)
        {
            return prediction.Prediction ? Sentiment.Positive : Sentiment.Negative;
        }
        else
        {
            return Sentiment.Neutral;
        }
    }
}
