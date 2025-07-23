using AventStack.ExtentReports.Model;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
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
    public class ProductsPage
    {
        private IWebDriver driver;
        
        public ProductsPage(IWebDriver driver)
        {
            this.driver = driver;
        }
        private By productLink = By.XPath("//header[@id='header']//li[2]");
        private By addToCartButton = By.XPath("(//a[contains(text(),'Add to cart')])[1]");
        private By viewProduct = By.XPath("(//a[contains(text(),'View Product')])[1]");
        private By viewCartLink = By.XPath("//u[contains(text(),'View Cart')]");
        private By searchText = By.Name("search");
        private By submitSearch = By.Id("submit_search");
        private By productNamesLocator = By.CssSelector(".productinfo.text-center p");
        private By quantityInput = By.Id("quantity");
        private By addToCartInDetail = By.XPath("//button[normalize-space()='Add to cart']");
        private By name = By.XPath("//input[@id='name']");
        private By email = By.XPath("//input[@id='email']");
        private By review = By.XPath("//textarea[@id='review']");
        private By submitReview = By.XPath("//button[@id='button-review']");
        private By aleartSuccess = By.CssSelector(".alert-success");
        private By addToCartButtonHover = By.XPath("//div[@class='col-sm-9 padding-right']//div[2]//div[1]//div[1]//div[2]//div[1]//a[1]");
        private By productHover = By.CssSelector(".product-overlay");
        private By cate= By.XPath("//a[normalize-space()='Women']");
        private By br = By.XPath("//a[@href='/brand_products/Polo']");
        private By active = By.CssSelector("li.active");
        private By continueShopping = By.XPath("//button[normalize-space()='Continue Shopping']");
        private By clickCartPage = By.XPath("//a[normalize-space()='Cart']");

        public void OpenProductPage()
        {
            driver.FindElement(productLink).Click(); 
        }

        public void ClickAddToCart()
        {
            driver.FindElement(addToCartButton).Click();
        }
        public void ClickCartPage()
        {
            driver.FindElement(clickCartPage).Click();
        }
        public void HoverProduct()
        {
            Actions actions = new Actions(driver);
            IWebElement productElement = driver.FindElement(productHover);
            actions.MoveToElement(productElement).Perform();

        }
        public void SelectCategory()
        {
            IWebElement categoryDropdown = driver.FindElement(cate);
            categoryDropdown.Click();
            IWebElement categoryOption = driver.FindElement(By.XPath("//div[@id='Women']//a[contains(text(),'Dress')]"));
            categoryOption.Click();
        }
        public void SelectBrand()
        {
            IWebElement brandDropdown = driver.FindElement(br);
            brandDropdown.Click();
          
        }
        public string GetActiveBrand()
        {
            IWebElement activeBrand = driver.FindElement(active);
            return activeBrand.Text.Trim();
        }

        public string GetCurrentCategory()
        {
            IWebElement activeCategory = driver.FindElement(active);
            return activeCategory.Text.Trim();
        }

        public void ClickAddToCartHover()
        {
            
            driver.FindElement(addToCartButtonHover).Click();
        }

        public void ClickViewCart()
        {
            driver.FindElement(viewCartLink).Click();
        }
        public void ClickContinueShopping()
        {
            driver.FindElement(continueShopping).Click();
        }
        public void ClickViewProduct()
        {
            driver.FindElement(viewProduct).Click();
        }
        public void ClickSearchBar()
        {
            driver.FindElement(searchText).Click();
        }
        public void SendKeySearchBar(String key)
        {
            driver.FindElement(searchText).SendKeys(key);

        }
        public void SubmitSearch()
        {
            driver.FindElement(submitSearch).Click();
        }
        public bool AreSearchResultsRelevant(string keyword)
        {
            var products = driver.FindElements(productNamesLocator);
            if (products.Count == 0)
            {
                return false; 
            }

            return products.All(p => p.Text.ToLower().Contains(keyword.ToLower()));
        }

        public List<string> GetSearchResultProductNames()
        {
            var products = driver.FindElements(productNamesLocator);
            return products.Select(p => p.Text).ToList();
        }
       
        public void SetQuantity(int quantity)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement quantityInput = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("quantity")));
            quantityInput.Click();
            quantityInput.Clear();  
            quantityInput.SendKeys(quantity.ToString());
        }
        public void AddToCartInDetail()
        {
            driver.FindElement(addToCartInDetail).Click();
        }
        public void WriteYourReview()
        {
            driver.FindElement(name).SendKeys("Truong");
            driver.FindElement(email).SendKeys("Truong@gmail.com");
            driver.FindElement(review).SendKeys("Good");

        }
        public void SubmitReview()
        {
            driver.FindElement(submitReview).Click();
        }
        public void CheckSuccessMessage()
        {

            WebDriverWait wait = new WebDriverWait (driver, TimeSpan.FromSeconds(10));
            IWebElement successAlert = wait.Until(ExpectedConditions.ElementIsVisible(aleartSuccess));

            string successMessage = successAlert.Text;
            Assert.That(successMessage, Is.EqualTo("Thank you for your review."), "Notify is error");
        }

        public void ScrollToProduct()
        {
            IWebElement element = driver.FindElement(viewProduct);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);

        }
       
    }
}
