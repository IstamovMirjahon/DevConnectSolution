using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DevConnect.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DevConnect.Application.Prompts;

/// <summary>
/// GPT-4o tomonidan qaytarilgan JSON ma'lumotni ushlab qoluvchi Model.
/// </summary>
public class ProfileAnalysisResponse
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = [];
    public string ExperienceLevel { get; set; } = string.Empty;
    public List<string> SuitableVacancies { get; set; } = [];

    [JsonPropertyName("portfolioUrl")]
    public string? PortfolioUrl { get; set; }
}

/// <summary>
/// OpenAI Chat Completions API orqali CV matnini tahlil qiladi.
/// ICvAnalyzerService ni implement qiladi.
/// </summary>
public class OpenAiProfileAnalyzerService(HttpClient httpClient, IConfiguration configuration)
    : ICvAnalyzerService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public const string SystemPrompt = @"
You are an expert IT Recruiter and Career Analyst AI.
Your task is to deeply analyze unstructured input (text, resume pieces, answers) provided by a job seeker and extract structured professional data.

Based on the candidate's input, deeply analyze and extract the following:
1. Title: Create the most appropriate professional job title (e.g., '.NET Backend Developer', 'UI/UX Designer').
2. Description: Write a clear, attractive, professional summary (3-4 sentences) highlighting their core expertise and background.
3. Skills: A list of specific technologies, tools, and soft skills they possess.
4. ExperienceLevel: Determine their level based on context (Intern, Junior, Middle, Senior, Lead).
5. SuitableVacancies: An array of 3-5 specific matching job vacancy titles that would perfectly match this candidate.
6. PortfolioUrl: If there is a personal website, GitHub, LinkedIn, or portfolio URL mentioned anywhere in the CV text, extract it here. Otherwise return null.

CRITICAL INSTRUCTION:
Respond ONLY with a raw JSON object exactly matching the structure below.
Do NOT include any markdown code blocks (like ```json), no greetings, no explanations.
Only output valid JSON.

{
    ""title"": ""string"",
    ""description"": ""string"",
    ""skills"": [""string""],
    ""experienceLevel"": ""string"",
    ""suitableVacancies"": [""string""],
    ""portfolioUrl"": ""string or null""
}";

    public async Task<ProfileAnalysisResponse?> AnalyzeCvAsync(string cvText, CancellationToken ct = default)
    {
        var apiKey = configuration["OpenAI:ApiKey"]
            ?? throw new InvalidOperationException("OpenAI:ApiKey is not configured.");
        var model = configuration["OpenAI:Model"] ?? "gpt-4o";

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model,
            messages = new[]
            {
                new { role = "system", content = SystemPrompt },
                new { role = "user",   content = cvText }
            },
            temperature = 0.3
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(
            "https://api.openai.com/v1/chat/completions", content, ct);

        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(responseJson);

        var gptText = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(gptText))
            return null;

        return JsonSerializer.Deserialize<ProfileAnalysisResponse>(gptText, JsonOptions);
    }

    /// <summary>
    /// Parse helper (eski kod bilan moslik uchun qoldirildi).
    /// </summary>
    public ProfileAnalysisResponse? ParseResponse(string gptJsonResponse)
    {
        try
        {
            return JsonSerializer.Deserialize<ProfileAnalysisResponse>(gptJsonResponse, JsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GPT dan qaytgan ma'lumotni o'qishda xatolik: {ex.Message}");
            return null;
        }
    }
}
