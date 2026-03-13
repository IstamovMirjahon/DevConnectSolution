using DevConnect.Domain.Entities;
using DevConnect.Domain.Enums;
using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Repositories;

public class UserRepository(DefaultContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await DbSet
            .FirstOrDefaultAsync(x => x.Email == email, ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        return await DbSet
            .AnyAsync(x => x.Email == email, ct);
    }

    public async Task<List<User>> GetByRoleAndTypeAsync(Role role, UserType? type, Profession? profession, CancellationToken ct)
    {
        var query = DbSet.Where(x => x.Role == role);

        if (type.HasValue)
            query = query.Where(x => x.Type == type.Value);

        if (profession.HasValue)
            query = query.Where(x => x.Profession == profession.Value);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task UpdateImageUrlAsync(Guid userId, string imageUrl, CancellationToken ct)
    {
        await DbSet
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.ImageUrl, imageUrl), ct);
    }

    public async Task UpdateCvProfileAsync(
        Guid userId,
        string cvUrl,
        string title,
        string description,
        List<string> skills,
        string experienceLevel,
        string? portfolioUrl,
        CancellationToken ct)
    {
        await DbSet
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.CvUrl, cvUrl)
                .SetProperty(u => u.Title, title)
                .SetProperty(u => u.Description, description)
                .SetProperty(u => u.Skills, skills)
                .SetProperty(u => u.ExperienceLevel, experienceLevel)
                .SetProperty(u => u.PortfolioUrl,
                    u => string.IsNullOrWhiteSpace(portfolioUrl) ? u.PortfolioUrl : portfolioUrl),
            ct);
    }
}
