namespace WebApp;

public class BlocklistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IBlockListService _BlockListService;

    public BlocklistMiddleware(RequestDelegate next, IBlockListService BlockListService)
    {
        _next = next;
        _BlockListService = BlockListService;
    }

    public async Task Invoke(HttpContext context)
    {
        var cookies = context.Request.Cookies;
        var blockedCookie = cookies.FirstOrDefault(cookie => _BlockListService.IsBlocked(cookie.Value));
        if (blockedCookie.Key is not null)
        {
            // Invalidate the session
            context.Response.Cookies.Delete(blockedCookie.Key);
            context.Session.Clear();

            // Redirect to login or home page
            context.Response.Redirect("/Account/Login");
            return;
        }

        await _next(context);
    }
}