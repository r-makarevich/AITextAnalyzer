using Microsoft.ML;
using TextAnalyzerAPI.Application.Models;
using TextAnalyzerAPI.Domain.Enums;

namespace TextAnalyzerAPI.Application.Services;

public class SentimentAnalysisService : ISentimentAnalysisService
{
    private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

    public SentimentAnalysisService()
    {
        MLContext mlContext = new MLContext(seed: 0);
        
        // Create training data with sentiment examples in multiple languages
        List<SentimentData> trainingData = new List<SentimentData>
        {
            // English - Positive
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
            
            // English - Negative
            new SentimentData { Text = "This is bad", Label = false },
            new SentimentData { Text = "I hate this", Label = false },
            new SentimentData { Text = "Terrible quality", Label = false },
            new SentimentData { Text = "Awful and horrible", Label = false },
            new SentimentData { Text = "Worst experience", Label = false },
            new SentimentData { Text = "Poor and disappointing", Label = false },
            new SentimentData { Text = "Sad and angry", Label = false },
            new SentimentData { Text = "Useless waste", Label = false },
            new SentimentData { Text = "Disgusting product", Label = false },
            new SentimentData { Text = "Very unhappy", Label = false },

            // Spanish - Positive
            new SentimentData { Text = "Esto es genial", Label = true },
            new SentimentData { Text = "Me encanta esto", Label = true },
            new SentimentData { Text = "Excelente trabajo", Label = true },
            new SentimentData { Text = "Increíble y maravilloso", Label = true },
            new SentimentData { Text = "Experiencia fantástica", Label = true },
            new SentimentData { Text = "Lo mejor que hay", Label = true },
            new SentimentData { Text = "Impresionante y perfecto", Label = true },
            new SentimentData { Text = "Hermoso y brillante", Label = true },
            
            // Spanish - Negative
            new SentimentData { Text = "Esto es malo", Label = false },
            new SentimentData { Text = "Odio esto", Label = false },
            new SentimentData { Text = "Calidad terrible", Label = false },
            new SentimentData { Text = "Horrible y espantoso", Label = false },
            new SentimentData { Text = "La peor experiencia", Label = false },
            new SentimentData { Text = "Pobre y decepcionante", Label = false },
            new SentimentData { Text = "Triste y enojado", Label = false },
            new SentimentData { Text = "Totalmente inútil", Label = false },

            // French - Positive
            new SentimentData { Text = "C'est génial", Label = true },
            new SentimentData { Text = "J'adore ça", Label = true },
            new SentimentData { Text = "Excellent travail", Label = true },
            new SentimentData { Text = "Incroyable et merveilleux", Label = true },
            new SentimentData { Text = "Expérience fantastique", Label = true },
            new SentimentData { Text = "La meilleure chose", Label = true },
            new SentimentData { Text = "Magnifique et brillant", Label = true },
            new SentimentData { Text = "Bon et heureux", Label = true },
            
            // French - Negative
            new SentimentData { Text = "C'est mauvais", Label = false },
            new SentimentData { Text = "Je déteste ça", Label = false },
            new SentimentData { Text = "Qualité terrible", Label = false },
            new SentimentData { Text = "Affreux et horrible", Label = false },
            new SentimentData { Text = "La pire expérience", Label = false },
            new SentimentData { Text = "Pauvre et décevant", Label = false },
            new SentimentData { Text = "Triste et en colère", Label = false },
            new SentimentData { Text = "Complètement inutile", Label = false },

            // German - Positive
            new SentimentData { Text = "Das ist großartig", Label = true },
            new SentimentData { Text = "Ich liebe das", Label = true },
            new SentimentData { Text = "Ausgezeichnete Arbeit", Label = true },
            new SentimentData { Text = "Erstaunlich und wunderbar", Label = true },
            new SentimentData { Text = "Fantastische Erfahrung", Label = true },
            new SentimentData { Text = "Das Beste überhaupt", Label = true },
            new SentimentData { Text = "Großartig und perfekt", Label = true },
            new SentimentData { Text = "Schön und brillant", Label = true },
            
            // German - Negative
            new SentimentData { Text = "Das ist schlecht", Label = false },
            new SentimentData { Text = "Ich hasse das", Label = false },
            new SentimentData { Text = "Schreckliche Qualität", Label = false },
            new SentimentData { Text = "Furchtbar und schrecklich", Label = false },
            new SentimentData { Text = "Die schlimmste Erfahrung", Label = false },
            new SentimentData { Text = "Arm und enttäuschend", Label = false },
            new SentimentData { Text = "Traurig und wütend", Label = false },
            new SentimentData { Text = "Völlig nutzlos", Label = false },

            // Italian - Positive
            new SentimentData { Text = "Questo è fantastico", Label = true },
            new SentimentData { Text = "Adoro questo", Label = true },
            new SentimentData { Text = "Ottimo lavoro", Label = true },
            new SentimentData { Text = "Straordinario e meraviglioso", Label = true },
            new SentimentData { Text = "Esperienza fantastica", Label = true },
            new SentimentData { Text = "La cosa migliore", Label = true },
            new SentimentData { Text = "Fantastico e perfetto", Label = true },
            new SentimentData { Text = "Bello e brillante", Label = true },
            
            // Italian - Negative
            new SentimentData { Text = "Questo è male", Label = false },
            new SentimentData { Text = "Odio questo", Label = false },
            new SentimentData { Text = "Qualità terribile", Label = false },
            new SentimentData { Text = "Orribile e terribile", Label = false },
            new SentimentData { Text = "La peggiore esperienza", Label = false },
            new SentimentData { Text = "Povero e deludente", Label = false },
            new SentimentData { Text = "Triste e arrabbiato", Label = false },
            new SentimentData { Text = "Completamente inutile", Label = false },

            // Portuguese - Positive
            new SentimentData { Text = "Isso é ótimo", Label = true },
            new SentimentData { Text = "Eu amo isso", Label = true },
            new SentimentData { Text = "Excelente trabalho", Label = true },
            new SentimentData { Text = "Incrível e maravilhoso", Label = true },
            new SentimentData { Text = "Experiência fantástica", Label = true },
            new SentimentData { Text = "A melhor coisa", Label = true },
            new SentimentData { Text = "Incrível e perfeito", Label = true },
            new SentimentData { Text = "Bonito e brilhante", Label = true },
            
            // Portuguese - Negative
            new SentimentData { Text = "Isso é ruim", Label = false },
            new SentimentData { Text = "Eu odeio isso", Label = false },
            new SentimentData { Text = "Qualidade terrível", Label = false },
            new SentimentData { Text = "Horrível e terrível", Label = false },
            new SentimentData { Text = "A pior experiência", Label = false },
            new SentimentData { Text = "Pobre e decepcionante", Label = false },
            new SentimentData { Text = "Triste e zangado", Label = false },
            new SentimentData { Text = "Completamente inútil", Label = false },

            // Dutch - Positive
            new SentimentData { Text = "Dit is geweldig", Label = true },
            new SentimentData { Text = "Ik hou hiervan", Label = true },
            new SentimentData { Text = "Uitstekend werk", Label = true },
            new SentimentData { Text = "Verbazingwekkend en prachtig", Label = true },
            new SentimentData { Text = "Fantastische ervaring", Label = true },
            new SentimentData { Text = "Het beste ooit", Label = true },
            new SentimentData { Text = "Geweldig en perfect", Label = true },
            new SentimentData { Text = "Mooi en briljant", Label = true },
            
            // Dutch - Negative
            new SentimentData { Text = "Dit is slecht", Label = false },
            new SentimentData { Text = "Ik haat dit", Label = false },
            new SentimentData { Text = "Verschrikkelijke kwaliteit", Label = false },
            new SentimentData { Text = "Vreselijk en verschrikkelijk", Label = false },
            new SentimentData { Text = "De slechtste ervaring", Label = false },
            new SentimentData { Text = "Slecht en teleurstellend", Label = false },
            new SentimentData { Text = "Verdrietig en boos", Label = false },
            new SentimentData { Text = "Volledig nutteloos", Label = false },

            // Russian - Positive
            new SentimentData { Text = "Это здорово", Label = true },
            new SentimentData { Text = "Мне это нравится", Label = true },
            new SentimentData { Text = "Отличная работа", Label = true },
            new SentimentData { Text = "Удивительно и прекрасно", Label = true },
            new SentimentData { Text = "Фантастический опыт", Label = true },
            new SentimentData { Text = "Лучшее из всего", Label = true },
            new SentimentData { Text = "Потрясающе и идеально", Label = true },
            new SentimentData { Text = "Красиво и блестяще", Label = true },
            
            // Russian - Negative
            new SentimentData { Text = "Это плохо", Label = false },
            new SentimentData { Text = "Я ненавижу это", Label = false },
            new SentimentData { Text = "Ужасное качество", Label = false },
            new SentimentData { Text = "Ужасно и страшно", Label = false },
            new SentimentData { Text = "Худший опыт", Label = false },
            new SentimentData { Text = "Плохо и разочаровывающе", Label = false },
            new SentimentData { Text = "Грустно и сердито", Label = false },
            new SentimentData { Text = "Совершенно бесполезно", Label = false },

            // Chinese - Positive
            new SentimentData { Text = "这太棒了", Label = true },
            new SentimentData { Text = "我喜欢这个", Label = true },
            new SentimentData { Text = "优秀的工作", Label = true },
            new SentimentData { Text = "令人惊叹和美妙", Label = true },
            new SentimentData { Text = "奇妙的体验", Label = true },
            new SentimentData { Text = "最好的东西", Label = true },
            new SentimentData { Text = "很棒和完美", Label = true },
            new SentimentData { Text = "美丽和辉煌", Label = true },
            
            // Chinese - Negative
            new SentimentData { Text = "这很糟糕", Label = false },
            new SentimentData { Text = "我讨厌这个", Label = false },
            new SentimentData { Text = "质量很差", Label = false },
            new SentimentData { Text = "可怕和糟糕", Label = false },
            new SentimentData { Text = "最糟糕的经历", Label = false },
            new SentimentData { Text = "差劲和令人失望", Label = false },
            new SentimentData { Text = "悲伤和愤怒", Label = false },
            new SentimentData { Text = "完全无用", Label = false },

            // Japanese - Positive
            new SentimentData { Text = "これは素晴らしい", Label = true },
            new SentimentData { Text = "これが大好きです", Label = true },
            new SentimentData { Text = "素晴らしい仕事", Label = true },
            new SentimentData { Text = "驚くべき素晴らしい", Label = true },
            new SentimentData { Text = "素晴らしい体験", Label = true },
            new SentimentData { Text = "最高のもの", Label = true },
            new SentimentData { Text = "素晴らしく完璧", Label = true },
            new SentimentData { Text = "美しく輝かしい", Label = true },
            
            // Japanese - Negative
            new SentimentData { Text = "これは悪い", Label = false },
            new SentimentData { Text = "これが嫌い", Label = false },
            new SentimentData { Text = "ひどい品質", Label = false },
            new SentimentData { Text = "ひどく恐ろしい", Label = false },
            new SentimentData { Text = "最悪の経験", Label = false },
            new SentimentData { Text = "悪くがっかり", Label = false },
            new SentimentData { Text = "悲しくて怒っている", Label = false },
            new SentimentData { Text = "完全に役に立たない", Label = false }
        };

        IDataView data = mlContext.Data.LoadFromEnumerable(trainingData);

        // Define the training pipeline
        IEstimator<ITransformer> pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: nameof(SentimentData.Label), 
                featureColumnName: "Features"));

        // Train the model
        ITransformer model = pipeline.Fit(data);
        
        // Create prediction engine
        _predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
    }

    public Sentiment AnalyzeSentiment(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Sentiment.Neutral;
        }

        SentimentData input = new SentimentData { Text = text };
        SentimentPrediction prediction = _predictionEngine.Predict(input);

        // Use ML.NET prediction result to determine sentiment
        // prediction.Prediction is true for positive sentiment, false for negative
        // prediction.Probability represents the probability of the positive class (true)
        // Use a threshold to determine if sentiment is strong enough, otherwise return neutral
        const float confidenceThreshold = 0.6f;
        
        if (prediction.Probability >= confidenceThreshold)
        {
            // High confidence in positive sentiment
            return Sentiment.Positive;
        }
        else if (prediction.Probability <= (1 - confidenceThreshold))
        {
            // High confidence in negative sentiment (probability of positive is low)
            return Sentiment.Negative;
        }
        else
        {
            // Low confidence, return neutral
            return Sentiment.Neutral;
        }
    }
}
