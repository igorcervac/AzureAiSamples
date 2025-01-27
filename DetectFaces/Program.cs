namespace DetectFaces;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.Vision.Face;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        var endPoint = config["endPoint"];
        if (string.IsNullOrEmpty(endPoint)){
            throw new Exception("Endpoint is required");
        }

        var key = config["key"] ?? throw new Exception("Key is required");
        if (string.IsNullOrEmpty(key)){
            throw new Exception("Key is required");
        }

        var client = new FaceClient(new Uri(endPoint), new AzureKeyCredential(key));

        const string inputFilename = "people.jpg"; 
        using var stream = new FileStream(inputFilename, FileMode.Open);
        FaceAttributeType[] faceAttributeTypes = { FaceAttributeType.Blur, FaceAttributeType.Mask, FaceAttributeType.HeadPose };
        
        Response<IReadOnlyList<FaceDetectionResult>> response = await client.DetectAsync(BinaryData.FromStream(stream), 
        FaceDetectionModel.Detection03, 
        FaceRecognitionModel.Recognition04, 
        returnFaceId: false, 
        returnFaceAttributes: faceAttributeTypes);

        stream.Close();
        var image = Image.FromFile(inputFilename);
        var graphics = Graphics.FromImage(image);
        var pen = new Pen(Color.LightGreen);
        var font = new Font("Arial", 4);
        var brush = new SolidBrush(Color.White);

        var indexedFaceDetectionResults = response.Value.Select((f, i) => (f, i));
        foreach (var (faceDetectionResult, index) in indexedFaceDetectionResults)
        {
            string faceName = $"Face {index + 1}";
            Console.WriteLine(faceName);

            var faceRectangle = faceDetectionResult.FaceRectangle;
            var rectangle = new Rectangle(faceRectangle.Left, faceRectangle.Top, faceRectangle.Width, faceRectangle.Height);
            Console.WriteLine(rectangle);
            graphics.DrawRectangle(pen, rectangle);
            graphics.DrawString(faceName, font, brush, new Point(faceRectangle.Left, faceRectangle.Top));

            Console.WriteLine(faceDetectionResult.FaceAttributes.HeadPose.Yaw);
            Console.WriteLine(faceDetectionResult.FaceAttributes.HeadPose.Pitch);
            Console.WriteLine(faceDetectionResult.FaceAttributes.HeadPose.Roll);

            Console.WriteLine(faceDetectionResult.FaceAttributes.Blur.BlurLevel);

            Console.WriteLine(faceDetectionResult.FaceAttributes.Mask.Type);
            Console.WriteLine();
        }

        const string outputFilename = "faces.jpg";
        image.Save(outputFilename);
    }
}
