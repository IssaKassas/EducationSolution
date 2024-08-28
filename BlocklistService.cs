namespace WebApp;

public interface IBlockListService
{
    void AddToBlocklist(string cookieValue);
    bool IsBlocked(string cookieValue);
}

public class BlockListService : IBlockListService
{
    private readonly HashSet<string> _blocklist = new HashSet<string>();

    public void AddToBlocklist(string cookieValue)
    {
        _blocklist.Add(cookieValue);
    }

    public bool IsBlocked(string cookieValue)
    {
        return _blocklist.Contains(cookieValue);
    }
}