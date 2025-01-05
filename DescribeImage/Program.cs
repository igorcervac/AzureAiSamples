namespace DescribeImage;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.Vision.ImageAnalysis;

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
        using var fileStream = new FileStream("building.jpg", FileMode.Open);
        
        ImageAnalysisResult result = await imageAnalysisClient.AnalyzeAsync(BinaryData.FromStream(fileStream), VisualFeatures.Caption | VisualFeatures.Tags);
        
        Console.WriteLine($"Caption: {result.Caption.Text}");
        Console.WriteLine($"Tags: {string.Join(", ", result.Tags.Values.Select(x => x.Name))}");
    }
}
