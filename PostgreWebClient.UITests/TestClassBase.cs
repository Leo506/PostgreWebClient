using OpenQA.Selenium;

namespace PostgreWebClient.UITests;

public class TestClassBase : IDisposable
{
    protected readonly IWebDriver _driver;

    public TestClassBase(IWebDriver driver)
    {
        _driver = driver;
    }

    protected void ClearCookie()
    {
        _driver.Manage().Cookies.DeleteAllCookies();
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}