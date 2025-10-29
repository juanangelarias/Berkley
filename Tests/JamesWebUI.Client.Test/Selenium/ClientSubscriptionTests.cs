using James.Shared;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace JamesWebUI.Client.Test.Selenium
{
    public class ClientSubscriptionTests(
        WebDriverFixture webDriverFixture,
        ConfigurationFixture configFixture,
        ITestOutputHelper output)
        : IClassFixture<WebDriverFixture>, IAssemblyFixture<ConfigurationFixture>
    {
        //private readonly ITestOutputHelper _output = output;
        //private readonly WebDriverFixture _webFixture = webDriverFixture;
        //private readonly ConfigurationFixture _configFixture = configFixture;

        [Fact]
        public async Task OnAddressModifiedClientSideNotification()
        {
            //Load 2 browsers and make sure they are running agency profile on the client side.
            var baseUrl = configFixture.Config["SeleniumBaseUrl"]!;
            try
            {
                //Launch both browsers on Agency 208 page
                await LoadAgencyPage(baseUrl, 208, webDriverFixture.ChromeDriver1);
                await RefreshUntilClientSide(webDriverFixture.ChromeDriver1);
                await LoadAgencyPage(baseUrl, 208, webDriverFixture.ChromeDriver2);
                await RefreshUntilClientSide(webDriverFixture.ChromeDriver2);

                //Enter edit mode and select Address2 on browser 1
                var editButton =
                    webDriverFixture.ChromeDriver1.FindElement(By.CssSelector("div[name='MainAddressDisplay'] button.rz-primary"));
                editButton.Click();
                var txtAddress2B1 = GetAddress2TextBox(webDriverFixture.ChromeDriver1);
                //Select Address2 on browser2
                var txtAddress2B2 = GetAddress2TextBox(webDriverFixture.ChromeDriver2);
                var initialValue2 = txtAddress2B2.Text;

                //Change and change back to fire subscription
                var initialValue1 = txtAddress2B1.Text;
                txtAddress2B1.Clear();
                txtAddress2B1.SendKeys( initialValue1 + "UnitTestValue");

                var saveButton =
                    webDriverFixture.ChromeDriver1.FindElement(By.CssSelector("div[name='MainAddressEdit'] button.rz-primary"));
                saveButton.Click();
                await Task.Delay(2000);

            }
            catch (Exception ex) when (ex.Message.Contains("ERR_CONNECTION_REFUSED"))
            {
                output.WriteLine("Test failed because server is not running at '{0}'.  If pointing to localhost, you must run JamesWebUI.Server.exe during tests", baseUrl);
                Assert.Fail();
            }
            catch (WebDriverException wde)
            {
                output.WriteLine(wde.ToText());
                throw;
            }
            await Task.Delay(10);
        }

        private async Task LoadAgencyPage(string baseUrl, int agencyNumber, IWebDriver driver)
        {
            var page = "/agency/" + agencyNumber;
            await driver.Navigate().GoToUrlAsync(baseUrl + page);
            //Check if login is required
            await Task.Delay(100);
            if (!driver.Url.StartsWith(baseUrl))
            {
                //At login screen
                var loginButton = driver.FindElement(By.CssSelector("form[data-provider=\"waad\"] button"));
                Assert.NotNull(loginButton);
                loginButton.Click();
                var maxWaits = 10;
                while (!driver.Url.StartsWith(baseUrl) && maxWaits-->0)
                    await Task.Delay(250);
            }
        }

        private async Task RefreshUntilClientSide(IWebDriver driver)
        {
            var maxRefreshes = 5;
            //Make sure page is loaded
            while (driver.FindElements(By.CssSelector("li.rz-tabview-selected.rz-state-focused")).Count ==0 && maxRefreshes-->0)
                await Task.Delay(1000 * (5-maxRefreshes));
            maxRefreshes = 5;
            while (driver.FindElements(By.ClassName("RunningServerSide")).Any() && maxRefreshes-->0)
            {
                output.WriteLine("Attempting to refresh to get client-side controls");
                await driver.Navigate().RefreshAsync();
                await Task.Delay(2000 * (5-maxRefreshes));
            }
        }

        private IWebElement GetAddress2TextBox(IWebDriver driver)
        {
            return driver.FindElement(By.CssSelector("input#Address2.rz-textbox"));
        }
    }
}
