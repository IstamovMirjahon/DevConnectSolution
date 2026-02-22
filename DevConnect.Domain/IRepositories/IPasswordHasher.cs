namespace DevConnect.Domain.IRepositories;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}