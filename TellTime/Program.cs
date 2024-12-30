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

        var key = configuration["key"];
        var region = configuration["region"];
        var speechConfig = SpeechConfig.FromSubscription(key, region);
        var synthetiser = new SpeechSynthesizer(speechConfig);

        var now = DateTime.Now;
        var timeString = $"Time is {now.Hour} : {now.Minute:D2}";
        await synthetiser.SpeakTextAsync(timeString);

        Console.ReadLine();
    }
}
