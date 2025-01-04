namespace TranscribeAudio;
using Microsoft.Extensions.Configuration;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

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
        var audioConfig = AudioConfig.FromWavFileInput("time.wav");
        
        var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        if (speechRecognitionResult.Reason == ResultReason.RecognizedSpeech)
        {
            Console.WriteLine(speechRecognitionResult.Text);
        }
    }
}
