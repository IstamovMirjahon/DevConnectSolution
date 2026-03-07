using System.Text.Json;

namespace DevConnect.Application.Prompts;

/// <summary>
/// GPT-4o tomonidan qaytarilgan JSON ma'lumotni ushlab qoluvchi Model/Class.
/// O'zingiz qanday ustunlar yaratgan bo'lsangiz, shularga moslashingiz mumkin.
/// </summary>
public class ProfileAnalysisResponse
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = new();
    public string ExperienceLevel { get; set; } = string.Empty;
    public List<string> SuitableVacancies { get; set; } = new();
}

/// <summary>
/// OpenAI Prompti va javobni ushlab oluvchi sodda xizmat (Service).
/// Buni o'zingizning asosiy Service ichida bemalol ishlata olasiz.
/// </summary>
public class OpenAiProfileAnalyzerService
{
    // 1. GPT-4o uchun maxsus yozilgan tizimli yo'riqnoma (System Prompt).
    public const string SystemPrompt = @"
You are an expert IT Recruiter and Career Analyst AI.
Your task is to deeply analyze unstructured input (text, resume pieces, answers) provided by a job seeker and extract structured professional data.

Based on the candidate's input, deeply analyze and extract the following:
1. Title: Create the most appropriate professional job title (e.g., '.NET Backend Developer', 'UI/UX Designer').
2. Description: Write a clear, attractive, professional summary (3-4 sentences) highlighting their core expertise and background.
3. Skills: A list of specific technologies, tools, and soft skills they possess.
4. ExperienceLevel: Determine their level based on context (Intern, Junior, Middle, Senior, Lead).
5. SuitableVacancies: An array of 3-5 specific matching job vacancy titles that would perfectly match this candidate.

CRITICAL INSTRUCTION: 
Respond ONLY with a raw JSON object exactly matching the structure below. 
Do NOT include any markdown code blocks (like ```json), no greetings, no explanations. 
Only output valid JSON.

{
    ""title"": ""string"",
    ""description"": ""string"",
    ""skills"": [""string""],
    ""experienceLevel"": ""string"",
    ""suitableVacancies"": [""string""]
}";

    // 2. Ushbu metod OpenAI dan qaytgan string ko'rinishidagi JSON'ni bizning C# class'imizga aylantirib beradi.
    // Siz mana shu yerdan qaytgan result.Title, result.Description larni bemalol bazangizga saqlaysiz.
    public ProfileAnalysisResponse? ParseResponse(string gptJsonResponse)
    {
        try
        {
            var options = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true // JSON dagi title bilan C# dagi Title ni bir-biriga tushishini ta'minlaydi
            };
            
            var result = JsonSerializer.Deserialize<ProfileAnalysisResponse>(gptJsonResponse, options);
            
            // Qaytgan 'result' ni Endi database ga Entities.User yoki shunga o'xshash jadvalingizga map qilib saqlaysiz
            return result;
        }
        catch (Exception ex)
        {
            // Xatolik bo'lsa konsolga yoki Loglarga yozishingiz mumkin.
            Console.WriteLine($""GPT dan qaytgan ma'lumotni o'qishda xatolik: {ex.Message}"");
            return null;
        }
    }
}
