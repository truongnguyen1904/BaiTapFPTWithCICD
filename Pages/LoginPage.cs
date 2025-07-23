using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace BaiTapFPT.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        private readonly By loginLink = By.XPath("//a[@href='/login']");
        private readonly By emailInput = By.XPath("//input[@data-qa='login-email']");
        private readonly By passwordInput = By.XPath("//input[@data-qa='login-password']");
        private readonly By loginButton = By.XPath("//button[@data-qa='login-button']");
        private readonly By errorMessage = By.XPath("//p[@style='color: red;']");
        private readonly By loggedInText = By.XPath("//a[contains(text(),'Logged in as')]");

        public void OpenLoginPage()
        {
            driver.FindElement(loginLink).Click();
        }

        public void EnterEmail(string email)
        {
            driver.FindElement(emailInput).Clear();
            driver.FindElement(emailInput).SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            driver.FindElement(passwordInput).Clear();
            driver.FindElement(passwordInput).SendKeys(password);
        }

        public void ClickLogin()
        {
            driver.FindElement(loginButton).Click();
        }

        public string GetLoggedInText()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(loggedInText));
            return driver.FindElement(loggedInText).Text;
        }

        public string GetErrorMessage()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(errorMessage));
            return driver.FindElement(errorMessage).Text;
        }

        public string GetPasswordRequiredAttribute()
        {
            return driver.FindElement(passwordInput).GetAttribute("required");
        }

        public string GetEmailRequiredAttribute()
        {
            return driver.FindElement(emailInput).GetAttribute("required");
        }
        public string GetEmailValidationMessage()
        {
            var email = driver.FindElement(emailInput);
            return (string)((IJavaScriptExecutor)driver)
                .ExecuteScript("return arguments[0].validationMessage;", email);
        }

        

    }
}
