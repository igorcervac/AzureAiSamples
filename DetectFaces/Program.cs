namespace DetectFaces;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.Vision.Face;
using System.Threading.Tasks;
using System.Drawing;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        var endPoint = config["endPoint"];
        var key = config["key"];

        var client = new FaceClient(new Uri(endPoint), new AzureKeyCredential(key));

        using var stream = new FileStream("people.jpg", FileMode.Open);
        FaceAttributeType[] faceAttributeTypes = new [] {FaceAttributeType.Blur, FaceAttributeType.Mask, FaceAttributeType.HeadPose};
        
        Response<IReadOnlyList<FaceDetectionResult>> response = await client.DetectAsync(BinaryData.FromStream(stream), 
        FaceDetectionModel.Detection03, 
        FaceRecognitionModel.Recognition04, 
        returnFaceId: false, 
        returnFaceAttributes: faceAttributeTypes);

        var indexedFaceDetectionResults = response.Value.Select((f, i) => (f, i));
        foreach (var (faceDetectionResult, index) in indexedFaceDetectionResults)
        {
            Console.WriteLine($"Face {index + 1}");

            var faceRectangle = faceDetectionResult.FaceRectangle;

            var rectangle = new Rectangle(faceRectangle.Left, faceRectangle.Top, faceRectangle.Width, faceRectangle.Height);
            Console.WriteLine(rectangle);

            Console.WriteLine(faceDetectionResult.FaceAttributes.HeadPose.Yaw);
            Console.WriteLine(faceDetectionResult.FaceAttributes.HeadPose.Pitch);
            Console.WriteLine(faceDetectionResult.FaceAttributes.HeadPose.Roll);

            Console.WriteLine(faceDetectionResult.FaceAttributes.Blur.BlurLevel);

            Console.WriteLine(faceDetectionResult.FaceAttributes.Mask.Type);
            Console.WriteLine();
        }
    }
}
