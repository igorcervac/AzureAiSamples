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
        var ssml = $@"
        <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
            <voice name='en-GB-RyanNeural'>
                {timeString}
                <break time='1000ms'/>
            </voice>
            <voice name='en-GB-LibbyNeural'>
                {timeString}
            </voice>
        </speak>
        ";

        SpeechSynthesisResult result = await speechSynthesizer.SpeakSsmlAsync(ssml);

        if (result.Reason != ResultReason.SynthesizingAudioCompleted)
        {
            Console.WriteLine(result.Reason);
        }

    }
}
