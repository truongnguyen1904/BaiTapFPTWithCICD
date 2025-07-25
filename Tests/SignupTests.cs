using BaiTapFPT.Helpers;
using BaiTapFPT.Models;
using BaiTapFPT.Pages;
using NUnit.Framework;
using System;

namespace BaiTapFPT.Tests
{
    [TestFixture("chrome")]
    [TestFixture("firefox")]
    [TestFixture("edge")]
    [Parallelizable(ParallelScope.All)]
    [Category("Signup")]
    public class SignupTests : BaseTest
    {
        private SignupPage signupPage;
        private SignupDetailsPage detailsPage;
        private SignupModel user;
        private readonly string browser;

        public SignupTests(string browser)
        {
            this.browser = browser;
        }

        //protected override string GetBrowser() => browser;
        [SetUp]
        public void TestSetUp()
        {
            signupPage = new SignupPage(driver.Value);
            detailsPage = new SignupDetailsPage(driver.Value);
            user = new SignupModel
            {
                Name = "Truong Tester",
                Email = $"truong{new Random().Next(1000, 9999)}@test.Value.com",
                Password = "123456",
                Day = "1",
                Month = "January",
                Year = "2000",
                FirstName = "Truong",
                LastName = "Nguyen",
                Address = "123 3/2 Street",
                City = "HCM",
                State = "HCM",
                Zipcode = "70000",
                MobileNumber = "0909123456"
            };
        }

        [Test]
        public void SignupWithValidInfoWithFullFieldDetail()
        {
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Signup page", () => signupPage.OpenSignupPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter name", () => signupPage.EnterName(user.Name));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter email", () => signupPage.EnterEmail(user.Email));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click signup", () => signupPage.ClickSignup());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Fill account info", () => detailsPage.FillAccountInfo(user.Password, user.Day, user.Month, user.Year));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Fill address info", () => detailsPage.FillAddressInfo(user.FirstName, user.LastName, user.Address, user.City, user.State, user.Zipcode, user.MobileNumber));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click create account", () => detailsPage.ClickCreateAccount());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify account created", () =>
            {
                Assert.That(driver.Value.PageSource, Does.Contain("Account Created!"), "Don't create an account");
            });
        }

        [Test]
        public void SignupWithValidInfoWithEmptyFieldRequired()
        {
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Signup page", () => signupPage.OpenSignupPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter name", () => signupPage.EnterName(user.Name));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter email", () => signupPage.EnterEmail(user.Email));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click signup", () => signupPage.ClickSignup());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Fill account info without password", () => detailsPage.FillAccountInfo("", user.Day, user.Month, user.Year));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Fill address info without required fields", () => detailsPage.FillAddressInfo("", "", "", "", "", "", ""));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click create account", () => detailsPage.ClickCreateAccount());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify password required", () =>
            {
                string isRequired = signupPage.GetPasswordRequiredAttribute();
                Assert.That(isRequired, Is.EqualTo("true"), "Password field is not required.");
            });
        }

        [Test]
        public void SignupWithEmptyName()
        {
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Signup page", () => signupPage.OpenSignupPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter empty name", () => signupPage.EnterName(""));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter email", () => signupPage.EnterEmail(user.Email));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click signup", () => signupPage.ClickSignup());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify name required", () =>
            {
                string isRequired = signupPage.GetNameRequiredAttribute();
                Assert.That(isRequired, Is.EqualTo("true"), "Name field is not required.");
            });
        }

        [Test]
        public void SignupWithEmptyEmail()
        {
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Signup page", () => signupPage.OpenSignupPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter name", () => signupPage.EnterName(user.Name));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter empty email", () => signupPage.EnterEmail(""));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click signup", () => signupPage.ClickSignup());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify email required", () =>
            {
                string isRequired = signupPage.GetEmailRequiredAttribute();
                Assert.That(isRequired, Is.EqualTo("true"), "Email field is not required.");
            });
        }

        [Test]
        public void SignupWithInvalidEmailFormat()
        {
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Signup page", () => signupPage.OpenSignupPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter name", () => signupPage.EnterName(user.Name));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter invalid email", () => signupPage.EnterEmail("truong123"));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click signup", () => signupPage.ClickSignup());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Check validation message", () =>
            {
                string validationMsg = signupPage.GetEmailValidationMessage();
                Assert.That(validationMsg, Is.Not.Empty, "Expected validation message not displayed for invalid email format.");
            });
        }

        [Test]
        public void SignupWithExistingEmail()
        {
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Signup page", () => signupPage.OpenSignupPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter name", () => signupPage.EnterName(user.Name));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter existing email", () => signupPage.EnterEmail("truongnguyen1904@gmail.com"));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click signup", () => signupPage.ClickSignup());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify existing email error", () =>
            {
                string errorMsg = signupPage.GetExistingEmailErrorMessage();
                Assert.That(errorMsg, Is.EqualTo("Email Address already exist!"), "Email Address not already exist");
            });
        }
    }
}
