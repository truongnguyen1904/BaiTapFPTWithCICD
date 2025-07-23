using BaiTapFPT.Helper;
using BaiTapFPT.Helpers;
using BaiTapFPT.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace BaiTapFPT.Tests
{
    [TestFixture, Category("Login")]
    public class LoginTests : BaseTest
    {
        private LoginPage loginPage;
        private string validEmail = "truongnguyen190404@gmail.com";
        private string validPassword = "123456";
        private string wrongPassword = "12345";
        private string wrongFormatEmail = "truong123";
        private string nonValidEmail = "truong1904@gmail.com";

        [SetUp]
        public void TestSetUp()
        {
            loginPage = new LoginPage(driver);
        }

        [Test]
        public void LoginWithValidAccount()
        {
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click login", () => loginPage.ClickLogin());

            string actualText = loginPage.GetLoggedInText();
            Assert.That(actualText, Does.Contain("Logged in as"), "Login failed with valid credentials.");

            TestHelper.CaptureStepAndScreenshot(test, driver, "Login successful", () => { });
        }

        [Test]
        public void LoginWithWrongPassword()
        {
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter wrong password", () => loginPage.EnterPassword(wrongPassword));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click login", () => loginPage.ClickLogin());

            string errorMsg = loginPage.GetErrorMessage();
            Assert.That(errorMsg, Is.EqualTo("Your email or password is incorrect!"), "Incorrect error message displayed");

            TestHelper.CaptureStepAndScreenshot(test, driver, "Login fail with wrong password", () => { });
        }

        [Test]
        public void LoginWithEmptyPassword()
        {
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter empty password", () => loginPage.EnterPassword(""));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click login", () => loginPage.ClickLogin());

            string isRequired = loginPage.GetPasswordRequiredAttribute();
            Assert.That(isRequired, Is.EqualTo("true"), "Password field is not required");

            TestHelper.CaptureStepAndScreenshot(test, driver, "Login fail with empty password", () => { });
        }

        [Test]
        public void LoginWithEmptyEmail()
        {
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter email empty", () => loginPage.EnterEmail(""));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click login", () => loginPage.ClickLogin());

            string isRequired = loginPage.GetEmailRequiredAttribute();
            Assert.That(isRequired, Is.EqualTo("true"), "Email field is not required");

            TestHelper.CaptureStepAndScreenshot(test, driver, "Login fail with empty email", () => { });
        }

        [Test]
        public void LoginWithNonRegisteredEmail()
        {
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter non registered email", () => loginPage.EnterEmail(nonValidEmail));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click login", () => loginPage.ClickLogin());

            string errorMsg = loginPage.GetErrorMessage();
            Assert.That(errorMsg, Is.EqualTo("Your email or password is incorrect!"), "Incorrect error message for non-registered email");

            TestHelper.CaptureStepAndScreenshot(test, driver, "Login fail with non registered email", () => { });
        }

        [Test]
        public void LoginWithInvalidEmailFormat()
        {
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter invalid format email", () => loginPage.EnterEmail(wrongFormatEmail));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click login", () => loginPage.ClickLogin());

            string validationMsg = loginPage.GetEmailValidationMessage();
            Assert.That(validationMsg, Is.Not.Empty, "Expected validation message not displayed for invalid email format.");

            TestHelper.CaptureStepAndScreenshot(test, driver, "Login fail with invalid email format", () => { });
        }
    }
}
