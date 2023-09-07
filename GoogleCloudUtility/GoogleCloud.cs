using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Grpc.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleCloudUtility.Helper;
using GoogleCloudUtility.Model;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.Extensions.Logging;
using Google.Api.Gax.Grpc.Rest;

namespace GoogleCloudUtility
{
    public class GoogleCloud
    {
        //public textToSpech.response textToSpech(string text)
        //{
        //    textToSpech.response _response;

        //    try
        //    {
        //        var credential = GoogleCredential.FromFile(@"C:\Users\ntsapp\AppData\Roaming\gcloud\application_default_credentials.json").CreateScoped(TextToSpeechClient.DefaultScopes);
        //        var channel = new Grpc.Core.Channel(TextToSpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());

        //        TextToSpeechClient client = TextToSpeechClient.Create();
        //        // The input can be provided as text or SSML.
        //        SynthesisInput input = new SynthesisInput
        //        {
        //            Text = text
        //        };

        //        // You can specify a particular voice, or ask the server to pick based
        //        // on specified criteria.
        //        VoiceSelectionParams voiceSelection = new VoiceSelectionParams
        //        {
        //            Name = "tr-TR-Wavenet-D",
        //            LanguageCode = "tr-TR",
        //            SsmlGender = SsmlVoiceGender.Neutral
        //        };

        //        // The audio configuration determines the output format and speaking rate.
        //        AudioConfig audioConfig = new AudioConfig
        //        {
        //            EffectsProfileId = { "telephony-class-application" },
        //            AudioEncoding = AudioEncoding.Mulaw,
        //            SampleRateHertz = 8000
        //        };

        //        SynthesizeSpeechResponse _googleResponse = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

        //        var _vawConverterResult = Helper.SoundConverter.ConvertWavTo8000Hz16BitMonoWav(_googleResponse.AudioContent.ToByteArray());

        //        _response = new textToSpech.response
        //        {
        //            status = true,
        //            AudioContent = _vawConverterResult,
        //            base64Content = _googleResponse.AudioContent.ToBase64(),

        //        };
        //    } catch
        //    {
        //        _response = new textToSpech.response
        //        {
        //            status = false
        //        };
        //    }

        //    return _response;
        //}


        //public async Task<SynthesizeSpeechResponse> textToSpechSsml(string ssml)
        //{
        //    string _json = @"{
        //      'client_id': '764086051850-6qr4p6gpi6hn506pt8ejuq83di341hur.apps.googleusercontent.com',
        //      'client_secret': 'd-FL95Q19q7MQmFpd7hHD0Ty',
        //      'quota_project_id': 'notional-arc-392017',
        //      'refresh_token': '1//0cJWxtvgVf-A7CgYIARAAGAwSNwF-L9IrMRuJiKooKsY66REFqSBzUs0ZJbEqjVEW9cfd7z7WMpq_TjgpDpDDPfjWmgVOeU-TMbg',
        //      'type': 'authorized_user'
        //    }";
        //    //GoogleCredential credential = GoogleCredential.FromJson(_json);

        //    //var credential = GoogleCredential.FromJson(_json).CreateScoped(TextToSpeechClient.DefaultScopes);

        //    ////var credential = GoogleCredential.FromFile(@"C:\service_account.json").CreateScoped(TextToSpeechClient.DefaultScopes);
        //    //var channel = new Grpc.Core.Channel(TextToSpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());

        //    var credential = GoogleCredential.FromJson(_json).CreateScoped(TextToSpeechClient.DefaultScopes);
        //    var channel = new Grpc.Core.Channel(TextToSpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());


        //    var client =  await TextToSpeechClient.CreateAsync(channel.ShutdownToken);
        //    //TextToSpeechClient client = TextToSpeechClient.Create();
        //    // The input can be provided as text or SSML.
        //    SynthesisInput input = new SynthesisInput
        //    {
        //        Ssml = ssml
        //    };

        //    // You can specify a particular voice, or ask the server to pick based
        //    // on specified criteria.
        //    VoiceSelectionParams voiceSelection = new VoiceSelectionParams
        //    {
        //        Name = "tr-TR-Wavenet-D",
        //        LanguageCode = "tr-TR",
        //        SsmlGender = SsmlVoiceGender.Neutral
        //    };

        //    // The audio configuration determines the output format and speaking rate.
        //    AudioConfig audioConfig = new AudioConfig
        //    {
        //        EffectsProfileId = { "telephony-class-application" },
        //        AudioEncoding = AudioEncoding.Mulaw,
        //        SampleRateHertz = 8000
        //    };

        //    SynthesizeSpeechResponse _googleResponse = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

        //    var _vawConverterResult = Helper.SoundConverter.ConvertWavTo8000Hz16BitMonoWav(_googleResponse.AudioContent.ToByteArray());

        //    return _googleResponse;

        //    //_response = new textToSpech.response
        //    //{
        //    //    status = true,
        //    //    AudioContent = _vawConverterResult,
        //    //    base64Content = _googleResponse.AudioContent.ToBase64(),

        //    //};

        //    //try
        //    //{

        //    //}
        //    //catch
        //    //{


        //    //    _response = new textToSpech.response
        //    //    {
        //    //        status = false
        //    //    };
        //    //}

        //    //return _response;
        //}


        public SynthesizeSpeechResponse textToSpechSsml(string ssml)
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

            var credential = GoogleCredential.FromJson(_json).CreateScoped(TextToSpeechClient.DefaultScopes);
            var channel = new Grpc.Core.Channel(TextToSpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());

            var client = new TextToSpeechClientBuilder
            {
                Credential = credential,
                GrpcAdapter = RestGrpcAdapter.Default
            }.Build();

            //var client = await TextToSpeechClient.CreateAsync(channel.ShutdownToken);
            //TextToSpeechClient client = TextToSpeechClient.Create();
            // The input can be provided as text or SSML.
            SynthesisInput input = new SynthesisInput
            {
                Ssml = ssml
            };

            // You can specify a particular voice, or ask the server to pick based
            // on specified criteria.
            VoiceSelectionParams voiceSelection = new VoiceSelectionParams
            {
                Name = "tr-TR-Wavenet-D",
                LanguageCode = "tr-TR",
                SsmlGender = SsmlVoiceGender.Neutral
            };

            // The audio configuration determines the output format and speaking rate.
            AudioConfig audioConfig = new AudioConfig
            {
                EffectsProfileId = { "telephony-class-application" },
                AudioEncoding = AudioEncoding.Mulaw,
                SampleRateHertz = 8000
            };

            SynthesizeSpeechResponse _googleResponse = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

            var _vawConverterResult = Helper.SoundConverter.ConvertWavTo8000Hz16BitMonoWav(_googleResponse.AudioContent.ToByteArray());

            return _googleResponse;

        }
    }
}
