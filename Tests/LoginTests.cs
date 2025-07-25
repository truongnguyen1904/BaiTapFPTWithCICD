using BaiTapFPT.Helper;
using BaiTapFPT.Helpers;
using BaiTapFPT.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.IO;


namespace BaiTapFPT.Tests
{

    [TestFixture("chrome")]
    [TestFixture("firefox")]
    [TestFixture("edge")]
    [Parallelizable(ParallelScope.All)]
    [Category("Login")]
    public class LoginTests : BaseTest
    {
        private LoginPage loginPage;
        private string validEmail = "truongnguyen190404@gmail.com";
        private string validPassword = "123456";
        private string wrongPassword = "12345";
        private string wrongFormatEmail = "truong123";
        private string nonValidEmail = "truong1904@gmail.com";
        private readonly string browser;

        public LoginTests(string browser) : base(browser) { }
        

        //protected override string GetBrowser( ) => browser;

        //[SetUp]
        //public void TestSetUp()
        //{
        //    loginPage = new LoginPage(driver.Value);
        //}
        private static string ExcelFilePath => Path.Combine(TestContext.CurrentContext.WorkDirectory, "LoginData.xlsx");

        public static IEnumerable<object[]> LoginTestDataFromExcel()
        {
            return ExcelDataHelper.ReadExcelData(ExcelFilePath, "Sheet1");
        }


        [Test]
        public void LoginWithValidAccount()
        {
            var loginPage = new LoginPage(driver.Value); 

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click login", () => loginPage.ClickLogin());

            string actualText = loginPage.GetLoggedInText();
            Assert.That(actualText, Does.Contain("Logged in as"), "Login failed with valid credentials.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Login successful", () => { });
        }

        [Test]
        public void LoginWithWrongPassword()
        {
            var loginPage = new LoginPage(driver.Value);

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter wrong password", () => loginPage.EnterPassword(wrongPassword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click login", () => loginPage.ClickLogin());

            string errorMsg = loginPage.GetErrorMessage();
            Assert.That(errorMsg, Is.EqualTo("Your email or password is incorrect!"), "Incorrect error message displayed");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Login fail with wrong password", () => { });
        }

        [Test]
        public void LoginWithEmptyPassword()
        {
            var loginPage = new LoginPage(driver.Value);

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter empty password", () => loginPage.EnterPassword(""));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click login", () => loginPage.ClickLogin());

            string isRequired = loginPage.GetPasswordRequiredAttribute();
            Assert.That(isRequired, Is.EqualTo("true"), "Password field is not required");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Login fail with empty password", () => { });
        }

        [Test]
        public void LoginWithEmptyEmail()
        {
            var loginPage = new LoginPage(driver.Value);

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter email empty", () => loginPage.EnterEmail(""));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click login", () => loginPage.ClickLogin());

            string isRequired = loginPage.GetEmailRequiredAttribute();
            Assert.That(isRequired, Is.EqualTo("true"), "Email field is not required");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Login fail with empty email", () => { });
        }

        [Test]
        public void LoginWithNonRegisteredEmail()
        {
            var loginPage = new LoginPage(driver.Value);

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter non registered email", () => loginPage.EnterEmail(nonValidEmail));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click login", () => loginPage.ClickLogin());

            string errorMsg = loginPage.GetErrorMessage();
            Assert.That(errorMsg, Is.EqualTo("Your email or password is incorrect!"), "Incorrect error message for non-registered email");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Login fail with non registered email", () => { });
        }

        [Test]
        public void LoginWithInvalidEmailFormat()
        {
            var loginPage = new LoginPage(driver.Value);

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter invalid format email", () => loginPage.EnterEmail(wrongFormatEmail));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click login", () => loginPage.ClickLogin());

            string validationMsg = loginPage.GetEmailValidationMessage();
            Assert.That(validationMsg, Is.Not.Empty, "Expected validation message not displayed for invalid email format.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Login fail with invalid email format", () => { });
        }

        [Test, TestCaseSource(nameof(LoginTestDataFromExcel))]
        public void LoginWithExcelData(string email, string password)
        {
            var loginPage = new LoginPage(driver.Value);

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, $"Enter email {email}", () => loginPage.EnterEmail(email));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter password", () => loginPage.EnterPassword(password));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click login", () => loginPage.ClickLogin());

            string actualText = loginPage.GetLoggedInText();
            if (actualText.Contains("Logged in as"))
            {
                test.Value.Info("Login successful with: " + email);
                Assert.Pass();
            }
            else
            {
                string errorMsg = loginPage.GetErrorMessage();
                test.Value.Warning("Login failed: " + errorMsg);
                Assert.Fail("Login failed with email: " + email);
            }
        }
    }
}
