using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapFPT.Pages
{
    public class SignupDetailsPage
    {
        private IWebDriver driver;

        public SignupDetailsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

       
        private By mrRadio = By.Id("id_gender1");
        private By passwordInput = By.Id("password");
        private By daySelect = By.Id("days");
        private By monthSelect = By.Id("months");
        private By yearSelect = By.Id("years");
        private By firstNameInput = By.Id("first_name");
        private By lastNameInput = By.Id("last_name");
        private By addressInput = By.Id("address1");
        private By countrySelect = By.Id("country");
        private By stateInput = By.Id("state");
        private By cityInput = By.Id("city");
        private By zipInput = By.Id("zipcode");
        private By phoneInput = By.Id("mobile_number");
        private By createAccountButton = By.XPath("//button[@data-qa='create-account']");

        public void FillAccountInfo(string password, string day, string month, string year)
        {
            driver.FindElement(mrRadio).Click();
            driver.FindElement(passwordInput).SendKeys(password);
            driver.FindElement(daySelect).SendKeys(day);
            driver.FindElement(monthSelect).SendKeys(month);
            driver.FindElement(yearSelect).SendKeys(year);
        }

        public void FillAddressInfo(string firstName, string lastName, string address, string state, string city, string zip, string phone)
        {
            driver.FindElement(firstNameInput).SendKeys(firstName);
            driver.FindElement(lastNameInput).SendKeys(lastName);
            driver.FindElement(addressInput).SendKeys(address);
            driver.FindElement(stateInput).SendKeys(state);
            driver.FindElement(cityInput).SendKeys(city);
            driver.FindElement(zipInput).SendKeys(zip);
            driver.FindElement(phoneInput).SendKeys(phone);
        }

        public void ClickCreateAccount()
        {
            driver.FindElement(createAccountButton).Click();
        }
    }
}
