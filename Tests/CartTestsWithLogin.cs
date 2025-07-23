using BaiTapFPT.Drivers;
using BaiTapFPT.Helpers;
using BaiTapFPT.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace BaiTapFPT.Tests
{
    [TestFixture, Category("Cart"), Parallelizable(ParallelScope.All)]
    public class CartTestsWithLogin : BaseTest
    {
        private LoginPage loginPage;
        private ProductsPage productsPage;
        private CartPage cartPage;

        private const string validEmail = "truongnguyen190404@gmail.com";
        private const string validPassword = "123456";

        [SetUp]
        public void TestSetUp()
        {
            loginPage = new LoginPage(driver);
            productsPage = new ProductsPage(driver);
            cartPage = new CartPage(driver);
        }

        private void LoginWithValidCredentials()
        {
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Enter valid email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Enter valid password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click login", () => loginPage.ClickLogin());

            string actualText = loginPage.GetLoggedInText();
            Assert.That(actualText, Does.Contain("Logged in as"), "Login failed with valid credentials.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Login successful", () => { });
            HideBottomAdBanner();
        }

        [Test]
        public void DeleteProductFromCart()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart", () => productsPage.ClickViewCart());

            Assert.That(cartPage.IsProductInCart(), "Product not found in cart after adding.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Delete product from cart", () => cartPage.DeleteQuantity());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Navigate to Home page", () => cartPage.OpenHome());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Reopen cart page", () => cartPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Verify product is removed", () => {
                bool isRemoved = cartPage.WaitUntilProductIsRemoved("Blue Top");
                Assert.That(isRemoved, "Product 'Blue Top' is still in cart after deletion.");
            });
        }

        [Test]
        public void ProceedToCheckout()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Add product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart", () => productsPage.ClickViewCart());

            Assert.That(cartPage.IsProductInCart(), "Product not found in cart.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Proceed to checkout", () => cartPage.ClickProceed());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Enter comment", () => cartPage.AddComment());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Place the order", () => cartPage.ClickPlaceOrder());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Enter card details", () => cartPage.AddCardDetail());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Confirm payment", () => cartPage.ClickPay());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Verify success message", () => {
                string actualMessage = cartPage.WaitForSuccessMessage();
                Assert.That(actualMessage, Is.EqualTo("Congratulations! Your order has been confirmed!"),
                    $"Expected order confirmation not found. Actual: '{actualMessage}'");
            });

            test.Value.Pass("Order confirmation message is displayed correctly.");
        }
        [Test]
        public void DeleteAllProductsFromCart()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Add product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart", () => productsPage.ClickViewCart());

            Assert.That(cartPage.IsProductInCart(), "Product not found in cart.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Delete all products from cart", () => cartPage.DeleteAllProducts());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Navigate to Home page", () => cartPage.OpenHome());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Reopen cart page", () => cartPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Verify cart is empty", () => {
                bool isCartEmpty = cartPage.IsCartEmpty();
                Assert.That(isCartEmpty, "Cart is not empty after removing all products.");
            });
        }

    }
}
