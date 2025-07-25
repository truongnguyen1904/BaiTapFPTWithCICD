using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
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
                case "edge":
                    driver.Value = InitEdgeDriver(appURL);
                    break;
                
                default:
                    Console.WriteLine("Browser is not supported. Opening Chrome by default.");
                    driver.Value = InitChromeDriver(appURL);
                    break;
            }
        }

        private static IWebDriver InitChromeDriver(string appURL)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("start-maximized");

            var chromeDriverService = ChromeDriverService.CreateDefaultService(
                @"C:\Users\HP\Downloads\BaiTapFPT\BaiTapFPT\driver");

            Console.WriteLine("Open Chrome");

            var localDriver = new ChromeDriver(chromeDriverService, options);
            localDriver.Manage().Window.Maximize();
            localDriver.Navigate().GoToUrl(appURL);
            localDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            localDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return localDriver;
        }
        private static IWebDriver InitEdgeDriver(string appURL)
        {
            var options = new EdgeOptions();

            if (Environment.GetEnvironmentVariable("JENKINS_HOME") != null)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                Console.WriteLine("Open Edge in headless mode (Jenkins)");
            }
            else
            {
                Console.WriteLine("Open Edge with GUI (local)");
            }

            var service = EdgeDriverService.CreateDefaultService(
                @"C:\Users\HP\Downloads\BaiTapFPT\BaiTapFPT\driver");

            var localDriver = new EdgeDriver(service, options);
            localDriver.Manage().Window.Maximize();
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

          var service = FirefoxDriverService.CreateDefaultService(@"C:\Users\HP\Downloads\BaiTapFPT\BaiTapFPT\driver"); // Thư mục chứa geckodriver.exe

            var localDriver = new FirefoxDriver(service, options); 

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
