namespace TranslateText;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.Translation.Text;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        var translatorKey = config["key"];
        var translatoRegion = config["region"];

        var textTranslationClient = new TextTranslationClient(new AzureKeyCredential(translatorKey), translatoRegion);
        var language = "en";
        var textToTranslate = "Bonjour Montreal!";

        Response<IReadOnlyList<TranslatedTextItem>> response = await textTranslationClient.TranslateAsync(language, textToTranslate);
        
        IReadOnlyList<TranslatedTextItem> translatedTextItems = response.Value;        
        var translatedTextItem = translatedTextItems[0];        
        Console.WriteLine($"'{textToTranslate}' was translated from {translatedTextItem.DetectedLanguage.Language} to {translatedTextItem.Translations[0].To} as '{translatedTextItem.Translations[0].Text}'");
    }
}
