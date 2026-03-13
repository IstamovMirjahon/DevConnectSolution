using DevConnect.Application.Prompts;

namespace DevConnect.Application.Interfaces;

public interface ICvAnalyzerService
{
    Task<ProfileAnalysisResponse?> AnalyzeCvAsync(string cvText, CancellationToken ct = default);
}
