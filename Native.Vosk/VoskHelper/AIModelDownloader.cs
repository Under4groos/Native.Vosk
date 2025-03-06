using Native.Vosk.Models;
using Newtonsoft.Json;

namespace Native.Vosk.Helper
{
    public class AIModelDownloader : IDisposable
    {
        // Remote location of the models and local folders
        public const string MODEL_PRE_URL = "https://alphacephei.com/vosk/models/";

        public const string MODEL_LIST_URL = MODEL_PRE_URL + "model-list.json";

        public string CACHE_VOSK = string.Empty;

        public string[] Tyes = { };


        public AIModelDownloader(string cachePath = ".cache/vosk")
        {
            CACHE_VOSK = cachePath;

            if (!Directory.Exists(CACHE_VOSK))
            {
                Directory.CreateDirectory(CACHE_VOSK);
                Console.WriteLine($"Create directory {Path.GetFullPath(cachePath)}");
            }
        }

        public async Task<string> GetVoskModels()
        {

            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://alphacephei.com/vosk/models/model-list.json");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }

        public async Task<List<VoskModel>> GetVoskModelsList()
        {
            List<VoskModel> lisVoskModel = new List<VoskModel>();
            try
            {
                var jsonListModels = await GetVoskModels();
                lisVoskModel = JsonConvert.DeserializeObject<List<VoskModel>>(jsonListModels);


                Tyes = lisVoskModel?.Select(m => m.type).GroupBy(x => x).Select(m => m.First()).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return lisVoskModel;


        }
        public async Task<List<VoskModel>> FindVoskModels(string lang = "ru")
        {
            var listModels = await GetVoskModelsList();
            return listModels.Where(m => m.lang?.ToLower() == lang.ToLower()).Select(m => m).ToList();
        }





        #region private
        private async Task _DownlaodModel(string fileUrl, string destinationPath)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(fileUrl))
                    {
                        response.EnsureSuccessStatusCode();
                        using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                                       fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                    }
                    Console.WriteLine("File downloaded successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        #endregion


        public void Dispose()
        {

        }
    }
}
