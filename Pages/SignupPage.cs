using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapFPT.Pages
{
    public class SignupPage
    {
        private IWebDriver driver;

        public SignupPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private By signupLoginLink = By.XPath("//a[@href='/login']");
        private By nameInput = By.XPath("//input[@data-qa='signup-name']");
        private By emailInput = By.XPath("//input[@data-qa='signup-email']");
        private By signupButton = By.XPath("//button[@data-qa='signup-button']");
        private By passwordInput = By.XPath("//input[@id='password']");

        public void OpenSignupPage()
        {
            driver.FindElement(signupLoginLink).Click();
        }

        public void EnterName(string name)
        {
            driver.FindElement(nameInput).Clear();
            driver.FindElement(nameInput).SendKeys(name);
        }

        public void EnterEmail(string email)
        {
            driver.FindElement(emailInput).Clear();
            driver.FindElement(emailInput).SendKeys(email);
        }

        public void ClickSignup()
        {
            driver.FindElement(signupButton).Click();
        }
        public string GetEmailRequiredAttribute()
        {
            return driver.FindElement(emailInput).GetAttribute("required");
        }
        public string GetNameRequiredAttribute()
        {
            return driver.FindElement(nameInput).GetAttribute("required");
        }
        public string GetPasswordRequiredAttribute()
        {
            return driver.FindElement(passwordInput).GetAttribute("required");
        }
        public string GetEmailValidationMessage()
        {
            var email = driver.FindElement(emailInput);
            return (string)((IJavaScriptExecutor)driver)
                .ExecuteScript("return arguments[0].validationMessage;", email);
        }
        public string GetExistingEmailErrorMessage()
        {
            return driver.FindElement(By.XPath("//p[normalize-space()='Email Address already exist!']")).Text;
        }

        public string GetValidationMessage(By locator)
        {
            var element = driver.FindElement(locator);
            return (string)((IJavaScriptExecutor)driver)
                .ExecuteScript("return arguments[0].validationMessage;", element);
        }



    }
}
