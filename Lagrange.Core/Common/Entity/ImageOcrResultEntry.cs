using System.Text.Json.Serialization;

namespace Lagrange.Core.Common.Entity
{
    [Serializable]
    public class ImageOcrResult
    {
        [JsonPropertyName("texts")] public List<TextDetection> Texts { get; set; }

        [JsonPropertyName("language")] public string Language { get; set; }

        public ImageOcrResult(List<TextDetection> texts, string language)
        {
            Texts = texts;
            Language = language;
        }
    }

    [Serializable]
    public class TextDetection
    {
        [JsonPropertyName("text")] public string Text { get; set; }

        [JsonPropertyName("confidence")] public int Confidence { get; set; }

        [JsonPropertyName("coordinates")] public List<Coordinate> Coordinates { get; set; }

        public TextDetection(string text, int confidence, List<Coordinate> coordinates)
        {
            Text = text;
            Confidence = confidence;
            Coordinates = coordinates;
        }
    }

    [Serializable]
    public class Coordinate
    {
        [JsonPropertyName("x")] public int X { get; set; }

        [JsonPropertyName("y")] public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}