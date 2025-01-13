namespace SummarizeText;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.TextAnalytics;
using System;
using System.IO;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        var endPoint = configuration["endPoint"];
        var key = configuration["key"];

        var textAnalyticsClient = new TextAnalyticsClient(new Uri(endPoint), new AzureKeyCredential(key));
        var textToAnalyze = File.ReadAllText("prompt-engineering.txt");
        ExtractiveSummarizeOperation operation = await textAnalyticsClient.ExtractiveSummarizeAsync(WaitUntil.Completed, new List<string>{textToAnalyze});
        
        await foreach(var extractiveSummarizeResults in operation.GetValuesAsync())
        {
            foreach(var extractiveSummarizeResult in extractiveSummarizeResults)
            {
                Console.WriteLine($"{string.Join(" ", extractiveSummarizeResult.Sentences.Select(x => x.Text))}");
            }
        }
    }
}
