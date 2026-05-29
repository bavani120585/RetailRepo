using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClaudeApiDemo
{
    public class VertexAiClient
    {
        private readonly string projectId;
        private readonly string region;
        private readonly string model;
        private readonly string apiEndpoint;
        private readonly HttpClient httpClient;

        public VertexAiClient(string projectId, string region, string model, string apiEndpoint)
        {
            this.projectId = projectId;
            this.region = region;
            this.model = model;
            this.apiEndpoint = apiEndpoint;
            this.httpClient = new HttpClient();
        }

        public async Task<string> SendMessageAsync(string message)
        {
            try
            {
                var accessToken = Environment.GetEnvironmentVariable("GCP_ACCESS_TOKEN");
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException(
                        "GCP_ACCESS_TOKEN environment variable not set. " +
                        "Run: export GCP_ACCESS_TOKEN=$(gcloud auth application-default print-access-token)");
                }

                var url = $"https://{apiEndpoint}/v1/projects/{projectId}/locations/{region}/publishers/anthropic/models/{model}:rawPredict";

                var request = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = message
                        }
                    },
                    max_tokens = 1024
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    error = ex.Message
                });
            }
        }
    }
}
