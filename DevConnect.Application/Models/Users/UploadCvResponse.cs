namespace DevConnect.Application.Models.Users;

public class UploadCvResponse
{
    public string CvUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = [];
    public string ExperienceLevel { get; set; } = string.Empty;
    public List<string> SuitableVacancies { get; set; } = [];
    public string? PortfolioUrl { get; set; }
}
