namespace PetPortalApplication.Helpers;

public static class AppUrls
{
    public static string FrontendBase =>
        (Environment.GetEnvironmentVariable("FRONTEND_BASE_URL") ?? "http://localhost:5173").TrimEnd('/');
}
