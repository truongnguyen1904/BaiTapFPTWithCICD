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

    private static ThreadLocal<ExtentTest> parentTest = new ThreadLocal<ExtentTest>();
    protected ThreadLocal<ExtentTest> test = new ThreadLocal<ExtentTest>();

    protected ExtentReports extent;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        extent = ExtentReportManager.GetInstance();

        string className = GetType().Name;
        parentTest.Value = extent.CreateTest(className);
    }

    [SetUp]
    public void SetUp()
    {
        // Init driver cho từng thread
        CreateDriver.SetDriver("chrome", "https://automationexercise.com/");
        driver = CreateDriver.GetDriver();

        // Tạo node test.Value riêng cho từng test.Value
        string testName = TestContext.CurrentContext.Test.MethodName;
        test.Value = parentTest.Value.CreateNode(testName);
    }

    protected void HideBottomAdBanner()
    {
        try
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.querySelectorAll('iframe').forEach(e => e.style.display = 'none');");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not hide ads: " + ex.Message);
        }
    }

    public void CaptureAndAddScreenshot(string stepName)
    {
        string screenshotPath = ScreenshotHelper.CaptureScreenshot(driver, stepName);
        test.Value.AddScreenCaptureFromPath(screenshotPath);
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            string screenshotPath = ScreenshotHelper.CaptureScreenshot(driver, TestContext.CurrentContext.Test.Name);
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

        CreateDriver.QuitDriver();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        extent.Flush();
    }
}
