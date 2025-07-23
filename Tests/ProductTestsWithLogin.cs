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
    [TestFixture, Category("ProductWithLogin"), Parallelizable(ParallelScope.All)]
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
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Enter email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click login", () => loginPage.ClickLogin());

            string actualText = loginPage.GetLoggedInText();
            Assert.That(actualText, Does.Contain("Logged in as"), "Login failed with valid credentials.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Login successful", () => { });
            HideBottomAdBanner();
        }
        private void SearchProductWithKeyword(string keyword)
        {
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click search bar", () => productsPage.ClickSearchBar());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, $"Enter keyword '{keyword}'", () => productsPage.SendKeySearchBar(keyword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click search", () => productsPage.SubmitSearch());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, $"Verify results for keyword '{keyword}'", () =>
            {
                Assert.That(productsPage.AreSearchResultsRelevant(keyword.ToLower()), $"Search results are not relevant to '{keyword}'");
            });

            var productNames = productsPage.GetSearchResultProductNames();
            foreach (var name in productNames)
            {
                test.Value.Info($"Found product: {name}");
            }
        }

        [Test]

        public void AddProduct()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Check product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void AddProductDouble()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart page", () => productsPage.ClickCartPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Delete all product in cart", () => cartPage.DeleteAllProducts());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Add first product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click continue shopping", () => productsPage.ClickContinueShopping());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Add same product again", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Verify product is in cart with quantity 2", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product not found in cart");
                cartPage.ProductQuantity(2);
            });
        }

        [Test]
        public void AddProductByCategory()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Select category 'MEN'", () => productsPage.SelectCategory());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Verify selected category", () =>
            {
                Assert.That(productsPage.GetCurrentCategory(), Is.EqualTo("Women > Dress"), "Incorrect category selected");
            });

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Check product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product not found in cart");
            });
        }

        [Test]
        public void AddProductByBrand()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Select brand 'POLO'", () => productsPage.SelectBrand());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Verify active brand", () =>
            {
                Assert.That(productsPage.GetActiveBrand(), Is.EqualTo("Polo"), "Incorrect brand selected");
            });
        }

        [Test]
        public void AddProductWhenHover()
        {
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Hover product", () => productsPage.HoverProduct());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click add to cart when hover", () => productsPage.ClickAddToCartHover());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Check product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProduct()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
        }

        [Test]
        public void SearchProductAndAddToCart()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());

            SearchProductWithKeyword(key);
            HideBottomAdBanner();


            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Add product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "View cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProductThenViewProductAndWriteReview()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
            HideBottomAdBanner();


            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Write review", () => productsPage.WriteYourReview());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Submit review", () => productsPage.SubmitReview());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Check success message", () => productsPage.CheckSuccessMessage());
        }

        [Test]
        public void SearchProductThenViewProductAndChangeQuantity()
        {
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart page", () => productsPage.ClickCartPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Delete all product in cart", () => cartPage.DeleteAllProducts());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Set quantity", () => productsPage.SetQuantity(qualiti));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Add to cart", () => productsPage.AddToCartInDetail());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver, "Verify quantity in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product not found in cart");
                cartPage.ProductQuantity(qualiti);
            });
        }
    }
}

