using Google.Api.Gax.Grpc.Rest;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robi_N_WebAPI.Utility;
using Image = Google.Cloud.Vision.V1.Image;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class VisionAIController : ControllerBase
    {

        private readonly AIServiceDbContext _db;
        private readonly IConfiguration _configuration;

        public VisionAIController(AIServiceDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }


        [HttpGet]
        public IActionResult getResult()
        {
            string _json = @"{
              'client_id': '764086051850-6qr4p6gpi6hn506pt8ejuq83di341hur.apps.googleusercontent.com',
              'client_secret': 'd-FL95Q19q7MQmFpd7hHD0Ty',
              'quota_project_id': 'notional-arc-392017',
              'refresh_token': '1//0cJWxtvgVf-A7CgYIARAAGAwSNwF-L9IrMRuJiKooKsY66REFqSBzUs0ZJbEqjVEW9cfd7z7WMpq_TjgpDpDDPfjWmgVOeU-TMbg',
              'type': 'authorized_user'
            }";
            //GoogleCredential credential = GoogleCredential.FromJson(_json);

            //var credential = GoogleCredential.FromJson(_json).CreateScoped(TextToSpeechClient.DefaultScopes);

            ////var credential = GoogleCredential.FromFile(@"C:\service_account.json").CreateScoped(TextToSpeechClient.DefaultScopes);
            //var channel = new Grpc.Core.Channel(TextToSpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());

            var credential = GoogleCredential.FromJson(_json).CreateScoped();



            //var client = new TextToSpeechClientBuilder
            //{
            //    Credential = credential,
            //    GrpcAdapter = RestGrpcAdapter.Default
            //}.Build();


            var client = new ImageAnnotatorClientBuilder
            {
                Credential = credential,
                GrpcAdapter = RestGrpcAdapter.Default
            }.Build();


            //ImageAnnotatorClient client = ImageAnnotatorClientBuilder {
            //    crede
            //}

            Image image1 = Image.FromFile(@"C:\Users\baris.sakizli\Downloads\BDH Servis Formları\bicycle_example.png");
            string poly = String.Empty;
            IReadOnlyList<LocalizedObjectAnnotation> annotations = client.DetectLocalizedObjects(image1);

            foreach (LocalizedObjectAnnotation annotation in annotations)
            {
                 poly = string.Join(" - ", annotation.BoundingPoly.NormalizedVertices.Select(v => $"({v.X}, {v.Y})"));
                Console.WriteLine(
                    $"Name: {annotation.Name}; ID: {annotation.Mid}; Score: {annotation.Score}; Bounding poly: {poly}");
            }

            return Ok(poly);
        }
    }
}
