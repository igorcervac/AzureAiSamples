namespace TellTime;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.CognitiveServices.Speech;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var speechKey = configuration["key"];
        var speechRegion = configuration["region"];
        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        var speechSynthesizer = new SpeechSynthesizer(speechConfig);

        var now = DateTime.Now;
        var timeString = $"Time is {now.Hour} : {now.Minute:D2}";
        await speechSynthesizer.SpeakTextAsync(timeString);

        Console.ReadLine();
    }
}
