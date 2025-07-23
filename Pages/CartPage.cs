using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaiTapFPT.Pages
{
    public class CartPage
    {
        private IWebDriver driver;

        public CartPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private By cartItemName = By.XPath("//td[@class='cart_description']/h4/a");
        private By cartQuantity = By.CssSelector(".cart_quantity button");
        private By deleteQuantity = By.XPath("//tr[@id='product-1']//a[@class='cart_quantity_delete']");
        private By homePage = By.XPath("//ul[@class='nav navbar-nav']//a[normalize-space()='Home']");
        private By viewCartLink = By.XPath("//a[normalize-space()='Cart']");
        private By proceedToCheckoutButton = By.XPath("//a[normalize-space()='Proceed To Checkout']");
        private By comment = By.XPath("//textarea[@name='message']");
        private By placeOrderButton = By.XPath("//a[normalize-space()='Place Order']");
        private By nameCard= By.XPath("//input[@name='name_on_card']");
        private By numberCard= By.XPath("//input[@name='card_number']");
        private By CVCCard= By.XPath("//input[@placeholder='ex. 311']");
        private By expirationCard= By.XPath("//input[@placeholder='MM']");
        private By yearCard= By.XPath("//input[@placeholder='YYYY']");
        private By payButton = By.XPath("//button[@id='submit']");
        private By message = By.CssSelector("div.alert-success.alert");

        public bool IsProductInCart()
        {
            return driver.FindElement(cartItemName).Displayed;
        }
        public void ProductQuantity(int expectedQuantity)
        {
            IWebElement cartquanti = driver.FindElement(cartQuantity);
            string actualQuantity = cartquanti.Text.Trim();

            Assert.That(actualQuantity, Is.EqualTo(expectedQuantity.ToString()), "The quantity isn't equal");
        }
        

        public void DeleteQuantity()
        {
            driver.FindElement(deleteQuantity).Click();
        }

        public void OpenHome()
        {
            driver.FindElement(homePage).Click();
        }
        public void ClickViewCart()
        {
            driver.FindElement(viewCartLink).Click();
        }
        public bool WaitUntilProductIsRemoved(string productName, int timeoutInSeconds = 10)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d =>
             {
                 var productElements = d.FindElements(By.XPath("//table[@id='cart_info_table']//td[@class='cart_description']//a"));
                 return !productElements.Any(p => p.Text.Trim().Equals(productName.Trim(), StringComparison.OrdinalIgnoreCase));
             });
        }
        public void ClickProceed()
        {
            driver.FindElement(proceedToCheckoutButton).Click();
        }
        public void AddComment()
        {
            driver.FindElement(comment).Click();
            driver.FindElement(comment).SendKeys("comment");
        }
        public void ClickPlaceOrder()
        {
            driver.FindElement(placeOrderButton).Click();
        }
        public void AddCardDetail()
        {
            driver.FindElement(nameCard).SendKeys("Truong");
            driver.FindElement(numberCard).SendKeys("02123");
            driver.FindElement(CVCCard).SendKeys("12");
            driver.FindElement(expirationCard).SendKeys("211");
            driver.FindElement(yearCard).SendKeys("2025");
        }
        public void ClickPay()
        {
            driver.FindElement(payButton).Click();

        }
        public string WaitForSuccessMessage(int timeoutSeconds = 10)
        {
           
             WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
             IWebElement successElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//p[text()='Congratulations! Your order has been confirmed!']")));
             return successElement.Text.Trim();
          
        }
        public void DeleteAllProducts()
        {
            var deleteButtons = driver.FindElements(By.CssSelector(".cart_quantity_delete"));
            foreach (var button in deleteButtons)
            {
                button.Click();
                Thread.Sleep(100);
            }
        }
        public bool IsCartEmpty()
        {
           
            var emptyCartMessage = driver.FindElement(By.Id("empty_cart"));
            return emptyCartMessage.Displayed;
          
            
        }





    }
}
