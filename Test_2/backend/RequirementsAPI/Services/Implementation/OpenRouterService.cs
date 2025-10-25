using System.Text;
using System.Text.Json;
using RequirementsAPI.Services.Interfaces;

namespace RequirementsAPI.Services.Implementation;

public class OpenRouterService : IOpenRouterService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<OpenRouterService> _logger;

    public OpenRouterService(HttpClient httpClient, IConfiguration configuration, ILogger<OpenRouterService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenRouter:ApiKey"] ?? throw new InvalidOperationException("OpenRouter API Key not configured");
        _logger = logger;
    }

    public async Task<string> AnalyzeRequirementsAsync(string requirementText)
    {
        try
        {
            var systemPrompt = @"You are an expert software analyst. Given software requirements, analyze them and generate a structured response in JSON format with the following structure:
{
  ""nombre"": ""Main process name"",
  ""descripcion"": ""Main process description"",
  ""subprocesos"": [
    {
      ""nombre"": ""Subprocess name"",
      ""descripcion"": ""Subprocess description"",
      ""casos_uso"": [
        {
          ""nombre"": ""Use case name"",
          ""descripcion"": ""Use case description"",
          ""actor_principal"": ""Main actor"",
          ""tipo_caso_uso"": 1,
          ""precondiciones"": ""Preconditions"",
          ""postcondiciones"": ""Postconditions"",
          ""criterios_de_aceptacion"": ""Acceptance criteria""
        }
      ]
    }
  ]
}

Notes:
- tipo_caso_uso: 1=Functional, 2=Non-Functional, 3=System
- Generate at least 1 subprocess and 1 use case per subprocess
- Be thorough and specific
- Response MUST be valid JSON only, no additional text";

            var requestBody = new
            {
                model = "deepseek/deepseek-chat",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = $"Analyze these requirements and generate the JSON structure:\n\n{requirementText}" }
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"OpenRouter API error: {response.StatusCode} - {errorContent}");
                throw new Exception($"OpenRouter API request failed: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"OpenRouter response: {responseContent}");

            var apiResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
            var generatedText = apiResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            return generatedText ?? throw new Exception("Empty response from OpenRouter API");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling OpenRouter API");
            throw;
        }
    }
}

