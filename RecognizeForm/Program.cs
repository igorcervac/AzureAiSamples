namespace ExtractFormData;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        var endPoint = config["endPoint"];
        var key = config["key"];

        DocumentAnalysisClient documentAnalysisClient = new DocumentAnalysisClient(new Uri(endPoint), new AzureKeyCredential(key));
        var fileUri = new Uri("https://github.com/MicrosoftLearning/mslearn-ai-document-intelligence/blob/main/Labfiles/01-prebuild-models/sample-invoice/sample-invoice.pdf?raw=true");
        
        AnalyzeDocumentOperation operation = await documentAnalysisClient.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-invoice", fileUri);
        
        AnalyzeResult result = operation.Value;
        foreach (var document in result.Documents)
        {
            if (document.Fields.TryGetValue("VendorName", out var vendorNameField))
            {
                Console.WriteLine($"VendorName: {vendorNameField.Value.AsString()}, with confidence {vendorNameField.Confidence}");
            }

            if (document.Fields.TryGetValue("CustomerName", out var customerNameField))
            {
                Console.WriteLine($"CustomerName: {customerNameField.Value.AsString()}, with confidence {customerNameField.Confidence}");
            }

            if (document.Fields.TryGetValue("InvoiceTotal", out var invoiceTotalField))
            {
                var currencyValue = invoiceTotalField.Value.AsCurrency();
                Console.WriteLine($"InvoiceTotal: {currencyValue.Symbol}{currencyValue.Amount}, with confidence {invoiceTotalField.Confidence}");
            }
        }
    }
}
