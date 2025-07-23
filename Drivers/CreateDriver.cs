using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Threading;

namespace BaiTapFPT.Drivers
{
    public class CreateDriver
    {
        private static IWebDriver driver;

        public static IWebDriver GetDriver()
        {
            return driver;
        }

        public static void SetDriver(string browserType, string appURL)
        {
            switch (browserType.ToLower())
            {
                case "chrome":
                    driver = InitChromeDriver(appURL);
                    break;
                case "firefox":
                    driver = InitFirefoxDriver(appURL);
                    break;
                default:
                    Console.WriteLine("Browser is not helper. Open Chrome");
                    driver = InitChromeDriver(appURL);
                    break;
            }
        }

        private static IWebDriver InitChromeDriver(string appURL)
        {
            Console.WriteLine("Open Chrome");
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(appURL);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
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
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(appURL);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        public static void QuitDriver()
        {
            Thread.Sleep(3000);
            if (driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }
    }
}
