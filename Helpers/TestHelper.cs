using AventStack.ExtentReports;
using BaiTapFPT.Helper;
using BaiTapFPT.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapFPT.Helpers
{
    public class TestHelper
    {
        public static void CaptureStepAndScreenshot(ExtentTest test, IWebDriver driver, string stepDescription, Action action)
        {
            var step = test.CreateNode(stepDescription);
            step.Info(stepDescription);

            action();

            ScreenshotHelper.AddScreenshotToReport(step, driver, stepDescription);
        }
    }
}
