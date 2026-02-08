using Microsoft.ML;
using TextAnalyzerAPI.Application.Models;

namespace TextAnalyzerAPI.Application.Services;

public class LanguageDetectionService : ILanguageDetectionService
{
    private readonly PredictionEngine<LanguageData, LanguagePrediction> _predictionEngine;
    private readonly string[] _languages = new[] { "English", "Spanish", "French", "German", "Italian", "Portuguese", "Dutch", "Russian", "Chinese", "Japanese" };

    public LanguageDetectionService()
    {
        MLContext mlContext = new MLContext(seed: 0);

        // Create training data with language examples
        List<LanguageData> trainingData = new List<LanguageData>
        {
            // English samples
            new LanguageData { Text = "Hello, how are you doing today?", Language = "English" },
            new LanguageData { Text = "The quick brown fox jumps over the lazy dog", Language = "English" },
            new LanguageData { Text = "I am learning machine learning with ML.NET", Language = "English" },
            new LanguageData { Text = "This is a wonderful day to be outside", Language = "English" },
            new LanguageData { Text = "The weather is beautiful today", Language = "English" },
            new LanguageData { Text = "I love programming and building applications", Language = "English" },
            new LanguageData { Text = "What time is it right now?", Language = "English" },
            new LanguageData { Text = "Thank you very much for your help", Language = "English" },

            // Spanish samples
            new LanguageData { Text = "Hola, ¿cómo estás hoy?", Language = "Spanish" },
            new LanguageData { Text = "Buenos días, ¿qué tal?", Language = "Spanish" },
            new LanguageData { Text = "Me gusta aprender idiomas nuevos", Language = "Spanish" },
            new LanguageData { Text = "La tecnología es increíble", Language = "Spanish" },
            new LanguageData { Text = "¿Dónde está la biblioteca?", Language = "Spanish" },
            new LanguageData { Text = "Muchas gracias por tu ayuda", Language = "Spanish" },
            new LanguageData { Text = "El clima está hermoso hoy", Language = "Spanish" },
            new LanguageData { Text = "Estoy aprendiendo español", Language = "Spanish" },

            // French samples
            new LanguageData { Text = "Bonjour, comment allez-vous?", Language = "French" },
            new LanguageData { Text = "Je suis très heureux aujourd'hui", Language = "French" },
            new LanguageData { Text = "La technologie est incroyable", Language = "French" },
            new LanguageData { Text = "J'aime apprendre de nouvelles langues", Language = "French" },
            new LanguageData { Text = "Merci beaucoup pour votre aide", Language = "French" },
            new LanguageData { Text = "Où est la bibliothèque?", Language = "French" },
            new LanguageData { Text = "Le temps est magnifique aujourd'hui", Language = "French" },
            new LanguageData { Text = "Je suis en train d'apprendre le français", Language = "French" },

            // German samples
            new LanguageData { Text = "Guten Tag, wie geht es Ihnen?", Language = "German" },
            new LanguageData { Text = "Ich lerne gerne neue Sprachen", Language = "German" },
            new LanguageData { Text = "Die Technologie ist erstaunlich", Language = "German" },
            new LanguageData { Text = "Vielen Dank für Ihre Hilfe", Language = "German" },
            new LanguageData { Text = "Wo ist die Bibliothek?", Language = "German" },
            new LanguageData { Text = "Das Wetter ist heute schön", Language = "German" },
            new LanguageData { Text = "Ich bin sehr glücklich", Language = "German" },
            new LanguageData { Text = "Guten Morgen, schönen Tag noch", Language = "German" },

            // Italian samples
            new LanguageData { Text = "Ciao, come stai oggi?", Language = "Italian" },
            new LanguageData { Text = "Mi piace imparare nuove lingue", Language = "Italian" },
            new LanguageData { Text = "La tecnologia è incredibile", Language = "Italian" },
            new LanguageData { Text = "Grazie mille per il tuo aiuto", Language = "Italian" },
            new LanguageData { Text = "Dov'è la biblioteca?", Language = "Italian" },
            new LanguageData { Text = "Il tempo è bellissimo oggi", Language = "Italian" },
            new LanguageData { Text = "Sono molto felice", Language = "Italian" },
            new LanguageData { Text = "Buongiorno, come va?", Language = "Italian" },

            // Portuguese samples
            new LanguageData { Text = "Olá, como você está hoje?", Language = "Portuguese" },
            new LanguageData { Text = "Eu gosto de aprender novas línguas", Language = "Portuguese" },
            new LanguageData { Text = "A tecnologia é incrível", Language = "Portuguese" },
            new LanguageData { Text = "Muito obrigado pela sua ajuda", Language = "Portuguese" },
            new LanguageData { Text = "Onde está a biblioteca?", Language = "Portuguese" },
            new LanguageData { Text = "O tempo está lindo hoje", Language = "Portuguese" },
            new LanguageData { Text = "Estou muito feliz", Language = "Portuguese" },
            new LanguageData { Text = "Bom dia, tudo bem?", Language = "Portuguese" },

            // Dutch samples
            new LanguageData { Text = "Hallo, hoe gaat het met je?", Language = "Dutch" },
            new LanguageData { Text = "Ik vind het leuk om nieuwe talen te leren", Language = "Dutch" },
            new LanguageData { Text = "De technologie is verbazingwekkend", Language = "Dutch" },
            new LanguageData { Text = "Hartelijk dank voor je hulp", Language = "Dutch" },
            new LanguageData { Text = "Waar is de bibliotheek?", Language = "Dutch" },
            new LanguageData { Text = "Het weer is prachtig vandaag", Language = "Dutch" },
            new LanguageData { Text = "Ik ben heel gelukkig", Language = "Dutch" },
            new LanguageData { Text = "Goedemorgen, fijne dag nog", Language = "Dutch" },

            // Russian samples
            new LanguageData { Text = "Привет, как дела сегодня?", Language = "Russian" },
            new LanguageData { Text = "Я люблю изучать новые языки", Language = "Russian" },
            new LanguageData { Text = "Технология потрясающая", Language = "Russian" },
            new LanguageData { Text = "Большое спасибо за вашу помощь", Language = "Russian" },
            new LanguageData { Text = "Где находится библиотека?", Language = "Russian" },
            new LanguageData { Text = "Погода сегодня прекрасная", Language = "Russian" },
            new LanguageData { Text = "Я очень счастлив", Language = "Russian" },
            new LanguageData { Text = "Доброе утро, хорошего дня", Language = "Russian" },

            // Chinese samples
            new LanguageData { Text = "你好，你今天怎么样？", Language = "Chinese" },
            new LanguageData { Text = "我喜欢学习新语言", Language = "Chinese" },
            new LanguageData { Text = "科技令人惊叹", Language = "Chinese" },
            new LanguageData { Text = "非常感谢你的帮助", Language = "Chinese" },
            new LanguageData { Text = "图书馆在哪里？", Language = "Chinese" },
            new LanguageData { Text = "今天天气很好", Language = "Chinese" },
            new LanguageData { Text = "我很高兴", Language = "Chinese" },
            new LanguageData { Text = "早上好，祝你愉快", Language = "Chinese" },

            // Japanese samples
            new LanguageData { Text = "こんにちは、今日はどうですか？", Language = "Japanese" },
            new LanguageData { Text = "新しい言語を学ぶのが好きです", Language = "Japanese" },
            new LanguageData { Text = "テクノロジーは素晴らしい", Language = "Japanese" },
            new LanguageData { Text = "ご協力ありがとうございます", Language = "Japanese" },
            new LanguageData { Text = "図書館はどこですか？", Language = "Japanese" },
            new LanguageData { Text = "今日は天気がいいですね", Language = "Japanese" },
            new LanguageData { Text = "とても幸せです", Language = "Japanese" },
            new LanguageData { Text = "おはようございます、良い一日を", Language = "Japanese" }
        };

        IDataView data = mlContext.Data.LoadFromEnumerable(trainingData);

        // Define the training pipeline for multi-class classification
        IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(LanguageData.Language))
            .Append(mlContext.Transforms.Text.FeaturizeText("Features", nameof(LanguageData.Text)))
            .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        // Train the model
        ITransformer model = pipeline.Fit(data);

        // Create prediction engine
        _predictionEngine = mlContext.Model.CreatePredictionEngine<LanguageData, LanguagePrediction>(model);
    }

    public string DetectLanguage(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return "Unknown";
        }

        LanguagePrediction prediction = _predictionEngine.Predict(new LanguageData { Text = text });
        string predictedLanguage = prediction.Language ?? "Unknown";
        
        // Validate and normalize the predicted language against known languages
        return _languages.FirstOrDefault(l => l.Equals(predictedLanguage, StringComparison.OrdinalIgnoreCase)) ?? "Unknown";
    }
}
