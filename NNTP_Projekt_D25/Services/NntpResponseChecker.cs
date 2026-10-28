namespace NNTP_Projekt_D25.Services;

public class NntpResponseChecker
{
    public static bool IsGreetingSuccessful(string? response)
    {
        return response?.StartsWith("200") == true;
    }

    public static bool ShouldSendPassword(string? response)
    {
        return response?.StartsWith("381") == true;
    }

    public static bool IsLoginSuccessful(string? response)
    {
        return response?.StartsWith("281") == true;
    }
}