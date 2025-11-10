using Android.App;
using Android.Graphics;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Parking.Mobile.Droid.DependencyService
{
    public class PlateRecognizer
    {
        private const string ApiUrl = "https://api.platerecognizer.com/v1/plate-reader/";
        private readonly string _apiToken;

        public PlateRecognizer(string apiToken)
        {
            _apiToken = apiToken ?? throw new ArgumentNullException(nameof(apiToken));
        }

        public async Task<string> ReadPlateAsync(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            try
            {
                // 🔹 Converte o bitmap para array de bytes
                using var stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 90, stream);
                var fileBytes = stream.ToArray();

                using var httpClient = new HttpClient();

                // 🔹 Cria o conteúdo multipart
                using var formData = new MultipartFormDataContent();

                // Adiciona a imagem (igual ao exemplo oficial)
                var imageContent = new ByteArrayContent(fileBytes);
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                formData.Add(imageContent, "upload", "image.jpg");

                // Adiciona região opcional (BR melhora precisão)
                formData.Add(new StringContent("br"), "regions");
                //formData.Add(new StringContent("vehicle"), "detection_mode");
                //formData.Add(new StringContent("true"), "mmc");

                // 🔹 Cabeçalho de autenticação (igual ao exemplo)
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_apiToken}");

                // 🔹 Faz a requisição POST
                var response = await httpClient.PostAsync(ApiUrl, formData);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Erro HTTP: {response.StatusCode}");
                    Console.WriteLine(json);
                    return null;
                }

                // 🔹 Lê o JSON retornado
                var result = JObject.Parse(json);
                var results = result["results"] as JArray;

                if (results != null && results.Count > 0)
                {
                    var plate = results[0]["plate"]?.ToString()?.ToUpper();
                    Console.WriteLine($"✅ Placa detectada pela API: {plate}");
                    return plate;
                }

                Console.WriteLine("⚠️ Nenhuma placa detectada.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro no PlateRecognizer: {ex.Message}");
                return null;
            }
        }
    }
}
