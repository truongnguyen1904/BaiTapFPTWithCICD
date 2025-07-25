using BaiTapFPT.Helpers;
using BaiTapFPT.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaiTapFPT.Tests
{
    [TestFixture("chrome")]
    [TestFixture("firefox")]
    [TestFixture("edge")]
    [Parallelizable(ParallelScope.All)]
    [Category("ProductWithLogin")]
    internal class ProductTestsWithLogin : BaseTest
    {
        //private ProductsPage productsPage;
        //private CartPage cartPage;
        //private LoginPage loginPage;
        private string validEmail = "truongnguyen190404@gmail.com";
        private string validPassword = "123456";

        private string key = "Men";
        private int qualiti = 5;

        private readonly string browser;

        public ProductTestsWithLogin(string browser) : base(browser) { }
    

    //protected override string GetBrowser( ) => browser;

    //[SetUp]
    //public void SetUp()
    //{
    //productsPage = new ProductsPage(driver.Value);
    //cartPage = new CartPage(driver.Value);
    //loginPage = new LoginPage(driver.Value);
    //}

    private void LoginWithValidCredentials()
        {
          
            var loginPage = new LoginPage(driver.Value);
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Login page", () => loginPage.OpenLoginPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter email", () => loginPage.EnterEmail(validEmail));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Enter password", () => loginPage.EnterPassword(validPassword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click login", () => loginPage.ClickLogin());

            string actualText = loginPage.GetLoggedInText();
            Assert.That(actualText, Does.Contain("Logged in as"), "Login failed with valid credentials.");

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Login successful", () => { });
            HideBottomAdBanner();
        }

        private void SearchProductWithKeyword(string keyword)
        {
            var productsPage = new ProductsPage(driver.Value);
            //var cartPage = new CartPage(driver.Value);
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click search bar", () => productsPage.ClickSearchBar());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, $"Enter keyword '{keyword}'", () => productsPage.SendKeySearchBar(keyword));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click search", () => productsPage.SubmitSearch());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, $"Verify results for keyword '{keyword}'", () =>
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
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Check product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void AddProductDouble()
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart page", () => productsPage.ClickCartPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Delete all product in cart", () => cartPage.DeleteAllProducts());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Add first product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click continue shopping", () => productsPage.ClickContinueShopping());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Add same product again", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify product is in cart with quantity 2", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product not found in cart");
                cartPage.ProductQuantity(2);
            });
        }

        [Test]
        public void AddProductByCategory()
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Select category 'MEN'", () => productsPage.SelectCategory());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify selected category", () =>
            {
                Assert.That(productsPage.GetCurrentCategory(), Is.EqualTo("Women > Dress"), "Incorrect category selected");
            });

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Check product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product not found in cart");
            });
        }

        [Test]
        public void AddProductByBrand()
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Select brand 'POLO'", () => productsPage.SelectBrand());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify active brand", () =>
            {
                Assert.That(productsPage.GetActiveBrand(), Is.EqualTo("Polo"), "Incorrect brand selected");
            });
        }

        [Test]
        public void AddProductWhenHover()
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Hover product", () => productsPage.HoverProduct());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart when hover", () => productsPage.ClickAddToCartHover());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Check product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProduct()
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
        }

        [Test]
        public void SearchProductAndAddToCart()
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());

            SearchProductWithKeyword(key);
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Add product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "View cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProductThenViewProductAndWriteReview()
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
            HideBottomAdBanner();

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Write review", () => productsPage.WriteYourReview());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Submit review", () => productsPage.SubmitReview());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Check success message", () => productsPage.CheckSuccessMessage());
        }

        [Test]
        public void SearchProductThenViewProductAndChangeQuantity()
        {
            var productsPage = new ProductsPage(driver.Value);
            var cartPage = new CartPage(driver.Value);
            LoginWithValidCredentials();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart page", () => productsPage.ClickCartPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Delete all product in cart", () => cartPage.DeleteAllProducts());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Set quantity", () => productsPage.SetQuantity(qualiti));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Add to cart", () => productsPage.AddToCartInDetail());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify quantity in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product not found in cart");
                cartPage.ProductQuantity(qualiti);
            });
        }
    }
}
