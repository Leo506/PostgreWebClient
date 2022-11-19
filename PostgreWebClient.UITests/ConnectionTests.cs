using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PostgreWebClient.UITests;

public class ConnectionTests : TestClassBase
{
    public ConnectionTests() : base(new ChromeDriver())
    {
    }

    [Fact]
    public void ConnectToDatabase_AllGood_ReturnsManipulationView()
    {
        // arrange
        _driver.Navigate().GoToUrl("https://localhost:7108/Connection");

        // act
        _driver.FindElement(By.Id("UserId")).SendKeys(ConnectionConstants.UserId);
        _driver.FindElement(By.Id("Password")).SendKeys(ConnectionConstants.Password);
        _driver.FindElement(By.Id("Database")).SendKeys(ConnectionConstants.Database);
        _driver.FindElement(By.Id("Connect")).Click();

        // assert
        _driver.Url.EndsWith("/manipulation").Should().BeTrue();
        
        ClearCookie();
    }
}