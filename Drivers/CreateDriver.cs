using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Threading;

namespace BaiTapFPT.Drivers
{
    public class CreateDriver
    {
        private static ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();

        public static IWebDriver GetDriver()
        {
            return driver.Value;
        }

        public static void SetDriver(string browserType, string appURL)
        {
            switch (browserType.ToLower())
            {
                case "chrome":
                    driver.Value = InitChromeDriver(appURL);
                    break;
                case "firefox":
                    driver.Value = InitFirefoxDriver(appURL);
                    break;
                
                default:
                    Console.WriteLine("Browser is not supported. Opening Chrome by default.");
                    driver.Value = InitFirefoxDriver(appURL);
                    break;
            }
        }

        private static IWebDriver InitChromeDriver(string appURL)
        {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");

            var chromeDriverService = ChromeDriverService.CreateDefaultService(
                @"C:\Users\HP\Downloads\BaiTapFPT\BaiTapFPT\driver");

            Console.WriteLine("Open Chrome");

            var localDriver = new ChromeDriver(chromeDriverService, options);
            localDriver.Navigate().GoToUrl(appURL);
            localDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            localDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return localDriver;
        }

        private static IWebDriver InitFirefoxDriver(string appURL)
        {
            Console.WriteLine("Open Firefox");

            var profile = new FirefoxProfile();
            profile.SetPreference("network.proxy.type", 0);

            var options = new FirefoxOptions
            {
                Profile = profile
            };

            var localDriver = new FirefoxDriver(options);
            localDriver.Manage().Window.Maximize();
            localDriver.Navigate().GoToUrl(appURL);
            localDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            localDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return localDriver;
        }

        public static void QuitDriver(IWebDriver driver)
        {
            if (driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }

    }
}
