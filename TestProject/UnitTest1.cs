namespace TestProject;
using NNTP_Projekt_D25;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        NNTP_Projekt_D25.Create_Connection("SimonAagaard", "asdgsgs");
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}