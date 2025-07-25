using BaiTapFPT.Drivers;
using BaiTapFPT.Helpers;
using BaiTapFPT.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace BaiTapFPT.Tests
{
    //Chay mot tesst voi nhieu trinh duyet khac nhau
    //[TestFixture("chrome")]
    //[TestFixture("firefox")]
    //[TestFixture("edge")]
    [TestFixture]
    //[Parallelizable(ParallelScope.All)]
    [Category("Cart")]
    public class CartTestsWithLogin :BaseTestFortestCase
    {
       

        private const string validEmail = "truongnguyen190404@gmail.com";
        private const string validPassword = "123456";
        //private readonly string browser;

        //public CartTestsWithLogin(string browser)
        //{
        //    this.browser = browser;
        //}

        //protected override string GetBrowser() => browser;


        private void LoginWithValidCredentials()
        {
            var loginPage = new LoginPage(driver.Value);
          
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter valid email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter valid password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click login", () => loginPage.ClickLogin());

            string actualText = loginPage.GetLoggedInText();
            Assert.That(actualText, Does.Contain("Logged in as"), "Login failed with valid credentials.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Login successful", () => { });
            HideBottomAdBanner();
        }
        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCase("firefox")]
        [TestCase("edge")]
        [TestCase("chrome")]

        public void DeleteProductFromCart(string browser)
        {

            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);


            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());
            Assert.That(cartPage.IsProductInCart(), "Product not found in cart after adding.");
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Delete product from cart", () => cartPage.DeleteQuantity());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Navigate to Home page", () => cartPage.OpenHome());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Reopen cart page", () => cartPage.ClickViewCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify product is removed", () =>
            {
                bool isRemoved = cartPage.WaitUntilProductIsRemoved("Blue Top");
                Assert.That(isRemoved, "Product 'Blue Top' is still in cart after deletion.");
            });

            // 5. Đóng trình duyệt
            CreateDriver.QuitDriver(driver.Value);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCase("firefox")]
        [TestCase("edge")]
        [TestCase("chrome")]

        public void ProceedToCheckout(String browser)
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);

            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Add product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            Assert.That(cartPage.IsProductInCart(), "Product not found in cart.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Proceed to checkout", () => cartPage.ClickProceed());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter comment", () => cartPage.AddComment());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Place the order", () => cartPage.ClickPlaceOrder());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter card details", () => cartPage.AddCardDetail());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Confirm payment", () => cartPage.ClickPay());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify success message", () =>
            {
                string actualMessage = cartPage.WaitForSuccessMessage();
                Assert.That(actualMessage, Is.EqualTo("Congratulations! Your order has been confirmed!"),
                    $"Expected order confirmation not found. Actual: '{actualMessage}'");
            });

            test.Value.Pass("Order confirmation message is displayed correctly.");
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCase("firefox")]
        [TestCase("chrome")]

        [TestCase("edge")]
        public void DeleteAllProductsFromCart(String browser)
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Add product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            Assert.That(cartPage.IsProductInCart(), "Product not found in cart.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Delete all products from cart", () => cartPage.DeleteAllProducts());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Navigate to Home page", () => cartPage.OpenHome());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Reopen cart page", () => cartPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify cart is empty", () =>
            {
                bool isCartEmpty = cartPage.IsCartEmpty();
                Assert.That(isCartEmpty, "Cart is not empty after removing all products.");
            });
        }
    }
}
