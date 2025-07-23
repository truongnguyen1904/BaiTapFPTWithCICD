using BaiTapFPT.Helpers;
using   BaiTapFPT.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaiTapFPT.Tests
{
    internal class ProductTestsWithLogin: BaseTest
    {

        private ProductsPage productsPage;
        private CartPage cartPage;
        private LoginPage loginPage;
        private string validEmail = "truongnguyen190404@gmail.com";
        private string validPassword = "123456";

        private String key = "Men";
        private int qualiti = 5;

        [SetUp]
        public void SetUp()
        {

            productsPage = new ProductsPage(driver);
            cartPage = new CartPage(driver);
            loginPage = new LoginPage(driver);
        }
        
        private void LoginWithValidCredentials()
        {
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click login", () => loginPage.ClickLogin());

            string actualText = loginPage.GetLoggedInText();
            Assert.That(actualText, Does.Contain("Logged in as"), "Login failed with valid credentials.");

            TestHelper.CaptureStepAndScreenshot(test, driver, "Login successful", () => { });
            HideBottomAdBanner();
        }
        private void SearchProductWithKeyword(string keyword)
        {
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click search bar", () => productsPage.ClickSearchBar());
            TestHelper.CaptureStepAndScreenshot(test, driver, $"Enter keyword '{keyword}'", () => productsPage.SendKeySearchBar(keyword));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click search", () => productsPage.SubmitSearch());

            TestHelper.CaptureStepAndScreenshot(test, driver, $"Verify results for keyword '{keyword}'", () =>
            {
                Assert.That(productsPage.AreSearchResultsRelevant(keyword.ToLower()), $"Search results are not relevant to '{keyword}'");
            });

            var productNames = productsPage.GetSearchResultProductNames();
            foreach (var name in productNames)
            {
                test.Info($"Found product: {name}");
            }
        }

        [Test]

        public void AddProduct()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test, driver, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Check product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void AddProductDouble()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart page", () => productsPage.ClickCartPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Delete all product in cart", () => cartPage.DeleteAllProducts());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Add first product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click continue shopping", () => productsPage.ClickContinueShopping());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Add same product again", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify product is in cart with quantity 2", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product not found in cart");
                cartPage.ProductQuantity(2);
            });
        }

        [Test]
        public void AddProductByCategory()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test, driver, "Select category 'MEN'", () => productsPage.SelectCategory());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify selected category", () =>
            {
                Assert.That(productsPage.GetCurrentCategory(), Is.EqualTo("Women > Dress"), "Incorrect category selected");
            });

            TestHelper.CaptureStepAndScreenshot(test, driver, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Check product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product not found in cart");
            });
        }

        [Test]
        public void AddProductByBrand()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test, driver, "Select brand 'POLO'", () => productsPage.SelectBrand());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify active brand", () =>
            {
                Assert.That(productsPage.GetActiveBrand(), Is.EqualTo("Polo"), "Incorrect brand selected");
            });
        }

        [Test]
        public void AddProductWhenHover()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test, driver, "Hover product", () => productsPage.HoverProduct());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click add to cart when hover", () => productsPage.ClickAddToCartHover());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Check product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProduct()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
        }

        [Test]
        public void SearchProductAndAddToCart()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());

            SearchProductWithKeyword(key);
            HideBottomAdBanner();


            TestHelper.CaptureStepAndScreenshot(test, driver, "Add product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "View cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProductThenViewProductAndWriteReview()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
            HideBottomAdBanner();


            TestHelper.CaptureStepAndScreenshot(test, driver, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Write review", () => productsPage.WriteYourReview());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Submit review", () => productsPage.SubmitReview());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Check success message", () => productsPage.CheckSuccessMessage());
        }

        [Test]
        public void SearchProductThenViewProductAndChangeQuantity()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart page", () => productsPage.ClickCartPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Delete all product in cart", () => cartPage.DeleteAllProducts());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Set quantity", () => productsPage.SetQuantity(qualiti));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Add to cart", () => productsPage.AddToCartInDetail());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify quantity in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product not found in cart");
                cartPage.ProductQuantity(qualiti);
            });
        }
    }
}

