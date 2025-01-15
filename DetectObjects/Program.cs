namespace DetectObjects;

using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Drawing;
using System.Drawing;

using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        var endPoint = configuration["endPoint"];
        var key = configuration["key"];

        var imageAnalysisClient = new ImageAnalysisClient(new Uri(endPoint), new AzureKeyCredential(key));
        const string inputFilename = "street.jpg";
        using var stream = new FileStream(inputFilename, FileMode.Open);
        ImageAnalysisResult imageAnalysisResult = await imageAnalysisClient.AnalyzeAsync(BinaryData.FromStream(stream), VisualFeatures.Objects);
        stream.Close();

        var font = new Font("Arial", 16);
        var brush = new SolidBrush(Color.WhiteSmoke);
        var pen = new Pen(Color.Cyan, 3);

        using var image = Image.FromFile(inputFilename);
        using var graphics = Graphics.FromImage(image);

        foreach (var detectedObject in imageAnalysisResult.Objects.Values)
        {           
            var box = detectedObject.BoundingBox;
            var rect = new Rectangle(box.X, box.Y, box.Width, box.Height);
            graphics.DrawRectangle(pen, rect);
            graphics.DrawString(detectedObject.Tags[0].Name, font, brush, rect.X, rect.Y);           
        }

        const string outputFilename = "objects.jpg";
        image.Save(outputFilename);
        Console.WriteLine($"Results saved in {outputFilename}");
    }
}
