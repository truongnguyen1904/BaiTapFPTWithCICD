using AventStack.ExtentReports;
using BaiTapFPT.Drivers;
using BaiTapFPT.Helper;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;

public class BaseTest
{
    protected static ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
    public IWebDriver Driver => driver.Value;
    private static ExtentTest parentTest;
    protected ThreadLocal<ExtentTest> test = new ThreadLocal<ExtentTest>();

    protected ExtentReports extent;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        extent = ExtentReportManager.GetInstance();

        string className = GetType().Name;
        parentTest = extent.CreateTest(className);
    }

    [SetUp]
    public void SetUp()
    {
        CreateDriver.SetDriver("chrome", "http://automationexercise.com/");
        driver.Value = CreateDriver.GetDriver(); 

        string testName = TestContext.CurrentContext.Test.MethodName;
        test.Value = parentTest.CreateNode(testName);
    }

    protected void HideBottomAdBanner()
    {
        try
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver.Value;
            js.ExecuteScript("document.querySelectorAll('iframe').forEach(e => e.style.display = 'none');");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not hide ads: " + ex.Message);
        }
    }

    public void CaptureAndAddScreenshot(string stepName)
    {
        string screenshotPath = ScreenshotHelper.CaptureScreenshot(driver.Value, stepName);
        test.Value.AddScreenCaptureFromPath(screenshotPath);
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            string screenshotPath = ScreenshotHelper.CaptureScreenshot(driver.Value, TestContext.CurrentContext.Test.Name);
            test.Value.Fail("Test Failed: " + message);
            if (screenshotPath != null)
                test.Value.AddScreenCaptureFromPath(screenshotPath);
        }
        else if (status == NUnit.Framework.Interfaces.TestStatus.Passed)
        {
            test.Value.Pass("Test Passed");
        }
        else
        {
            test.Value.Warning("Test had unexpected status: " + status);
        }

        CreateDriver.QuitDriver(driver.Value);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        extent.Flush();
    }
}
