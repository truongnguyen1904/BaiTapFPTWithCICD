using AventStack.ExtentReports;
using BaiTapFPT.Drivers;
using BaiTapFPT.Helper;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Concurrent;
using System.Threading;

public class BaseTest
{
    protected static ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
    public IWebDriver Driver => driver.Value;

    private static ExtentReports extent;
    protected static ThreadLocal<ExtentTest> parentTest = new ThreadLocal<ExtentTest>(() => null); // tránh null exception
    protected ThreadLocal<ExtentTest> test = new ThreadLocal<ExtentTest>();

    protected readonly string browser;

    public BaseTest(string browser)
    {
        this.browser = browser.ToLower();
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        extent = ExtentReportManager.GetInstance();
    }


    [SetUp]
    public void SetUp()
    {
        CreateDriver.SetDriver(browser, "https://automationexercise.com");
        driver.Value = CreateDriver.GetDriver();

        string parentKey = $"{GetType().Name} [{browser}]";

        if (parentTest.Value == null)
        {
            lock (typeof(BaseTest))
            {
                if (parentTest.Value == null)
                {
                    parentTest.Value = ExtentReportManager.GetInstance().CreateTest(parentKey);
                }
            }
        }

        string testName = TestContext.CurrentContext.Test.MethodName;
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
