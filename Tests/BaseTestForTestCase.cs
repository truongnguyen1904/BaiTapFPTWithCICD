using AventStack.ExtentReports;
using BaiTapFPT.Drivers;
using BaiTapFPT.Helper;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Concurrent;
using System.Threading;

public class BaseTestFortestCase
{
    protected static ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
    public IWebDriver Driver => driver.Value;

    protected static ExtentReports extent;
    protected static ConcurrentDictionary<string, ExtentTest> parentTests = new ConcurrentDictionary<string, ExtentTest>();

    protected ThreadLocal<ExtentTest> test = new ThreadLocal<ExtentTest>();
    protected ThreadLocal<ExtentTest> parentTest = new ThreadLocal<ExtentTest>();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        extent = ExtentReportManager.GetInstance();
    }

    [SetUp]
    public void SetUp()
    {
        // Lấy browser từ TestCase
        string browser = "chrome";
        var args = TestContext.CurrentContext.Test.Arguments;
        if (args.Length > 0 && args[0] is string arg)
        {
            browser = arg.ToLower();
        }

        // Khởi tạo driver
        CreateDriver.SetDriver(browser, "https://automationexercise.com");
        driver.Value = CreateDriver.GetDriver();

        // Tạo parent test và test node cho ExtentReport
        string className = GetType().Name;
        string parentKey = $"{className} [{browser}]";

        parentTest.Value = parentTests.GetOrAdd(parentKey, _ => extent.CreateTest(parentKey));
        string testName = TestContext.CurrentContext.Test.MethodName + $" ({browser})";

        test.Value = parentTest.Value.CreateNode(testName);
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
            test.Value.Warning("Test ended with unexpected status: " + status);
        }

        CreateDriver.QuitDriver(driver.Value);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        extent.Flush();
    }
}
