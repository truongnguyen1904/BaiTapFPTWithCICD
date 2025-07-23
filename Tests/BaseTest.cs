using AventStack.ExtentReports;
using BaiTapFPT.Drivers;
using BaiTapFPT.Helper;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;

public class BaseTest
{
    protected IWebDriver driver;
    protected ExtentReports extent;
    protected ExtentTest parentTest;  
    protected ExtentTest test;       

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        extent = ExtentReportManager.GetInstance();
    }

    [SetUp]
    public void SetUp()
    {
        string browserType = "chrome";
        string appURL = "https://automationexercise.com/";

        CreateDriver.SetDriver(browserType, appURL);
        driver = CreateDriver.GetDriver();

        string className = GetType().Name;
        parentTest = extent.CreateTest(className);

        string testName = TestContext.CurrentContext.Test.MethodName;
        test = parentTest.CreateNode(testName);   
    }

   
    public void HideBottomAdBanner()
    {
        try
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
            window.scrollBy(0, 200); 
            var banner = document.querySelector('.bottom-banner'); 
            if (banner) banner.style.display = 'none';
        ");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hide ad error: " + ex.Message);
        }
    }

    public void CaptureAndAddScreenshot(string stepName)
    {
        string screenshotPath = ScreenshotHelper.CaptureScreenshot(driver, stepName);

        test.AddScreenCaptureFromPath(screenshotPath);
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            string screenshotPath = ScreenshotHelper.CaptureScreenshot(driver, TestContext.CurrentContext.Test.Name);
            test.Fail("Test Failed: " + message);
            if (screenshotPath != null)
                test.AddScreenCaptureFromPath(screenshotPath);
        }
        else if (status == NUnit.Framework.Interfaces.TestStatus.Passed)
        {
            test.Pass("Test Passed");
        }
        else
        {
            test.Warning("Test had unexpected status: " + status);
        }


        CreateDriver.QuitDriver();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        extent.Flush();
    }
}
