using KeyShieldAPI.Services;
using Microsoft.Identity.Web;

namespace KeyShieldAPI.Middlewares;

public class GetOrCreateUtilisateurMiddleware(UtilisateurService utilisateurService) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            if ((context.User.Identity?.IsAuthenticated ?? false) == false)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                var objectId = context.User.GetObjectId();
                if (objectId == null)
                {
                    throw new ArgumentNullException(nameof(objectId), "User object ID cannot be null.");
                }
                utilisateurService.CurrentAppUserId = await utilisateurService.GetOrCreateUtilisateurAsync(objectId);
                await next(context);
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(ex.Message);
        }
    }
}