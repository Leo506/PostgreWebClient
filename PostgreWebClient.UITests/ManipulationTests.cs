using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PostgreWebClient.UITests;

public class ManipulationTests : TestClassBase
{
    public ManipulationTests() : base(new ChromeDriver())
    {
    }

    [Fact]
    public void OnManipulationPage_ThereAreSchemaList()
    {
        // arrange
        // connect to database
        _driver.Navigate().GoToUrl("https://localhost:7108/Connection");
        _driver.FindElement(By.Id("UserId")).SendKeys(ConnectionConstants.UserId);
        _driver.FindElement(By.Id("Password")).SendKeys(ConnectionConstants.Password);
        _driver.FindElement(By.Id("Database")).SendKeys(ConnectionConstants.Database);
        _driver.FindElement(By.Id("Connect")).Click();

        // act
        var schemaList = _driver.FindElement(By.Id("databaseInfo")).FindElements(By.XPath(".//*"));

        // assert
        schemaList.Count.Should().BeGreaterThan(2);
        
        ClearCookie();
    }
}