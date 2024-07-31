using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace JamesWebUI.Client.Test.Selenium
{
    public class WebDriverFixture :IDisposable
    {
        public ChromeDriver ChromeDriver1 { get; init; }
        public ChromeDriver ChromeDriver2 { get; init; }
        public WebDriverFixture()
        {
            //Set headless mode unless turned off
            var useHeadless = !Boolean.TryParse( InternalConfiguration.Config["SeleniumRunHeadless"], out var noDisplay) || noDisplay;

            //WebDriverManager
            var chromeOptions = new ChromeOptions();
            if (useHeadless)
                chromeOptions.AddArgument("headless");
            var driver = new DriverManager().SetUpDriver(new ChromeConfig());
            ChromeDriver1 = new ChromeDriver(driver, chromeOptions);
            ChromeDriver2 = new ChromeDriver(driver, chromeOptions);
        }
        public void Dispose()
        {
            ChromeDriver1.Quit();
            ChromeDriver2.Quit();
            ChromeDriver1.Dispose();
            ChromeDriver2.Dispose();
        }
    }
}
