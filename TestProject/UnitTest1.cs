using NNTP_Projekt_D25.Services;
using NNTP_Projekt_D25.Views;

namespace TestProject;
using NNTP_Projekt_D25;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        NntpService.Create_Connection("SimonAagaard", "pjlmewywy");
        NntpResponseChecker responseChecker = new NntpResponseChecker();
    }

    [Test]
    public void IsConnectedToServer()
    {
        bool actual = NntpResponseChecker.IsGreetingSuccessful(NntpService.greetingResponse);
        Assert.That(actual, Is.True);
    }
    
    [Test]
    public void IsLoggedIn()
    {
        bool actual = NntpResponseChecker.IsLoginSuccessful(NntpService.loginResponse);
        Assert.That(actual, Is.True);
    }
    
}