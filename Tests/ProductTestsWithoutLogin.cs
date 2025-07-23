using BaiTapFPT.Helpers;
using BaiTapFPT.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaiTapFPT.Tests
{
    [TestFixture, Category("ProductWithOutLogin")]
    public class ProductTestsWithoutLogin : BaseTest
    {
        private ProductsPage productsPage;
        private CartPage cartPage;
        private string key = "Men";
        private int qualiti = 5;

        [SetUp]
        public void SetUp()
        {
            productsPage = new ProductsPage(driver);
            cartPage = new CartPage(driver);
        }

        [Test]
        public void AddProduct()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void AddProductDouble()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click continue shopping", () => productsPage.ClickContinueShopping());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click add to cart again", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Check quantity in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
                cartPage.ProductQuantity(2);
            });
        }

        [Test]
        public void AddProductByCategory()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Select category 'MEN'", () => productsPage.SelectCategory());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify selected category", () =>
            {
                Assert.That(productsPage.GetCurrentCategory(), Is.EqualTo("Women > Dress"), "Category is incorrect");
            });
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void AddProductByBrand()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Select brand 'POLO'", () => productsPage.SelectBrand());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify selected brand", () =>
            {
                Assert.That(productsPage.GetActiveBrand(), Is.EqualTo("Polo"), "Brand is incorrect");
            });
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void AddProductWhenHover()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Hover over product", () => productsPage.HoverProduct());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Click add to cart (hover)", () => productsPage.ClickAddToCartHover());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProduct()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
        }

        [Test]
        public void SearchProductAndAddToCart()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);

            TestHelper.CaptureStepAndScreenshot(test, driver, "Add product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProductThenViewProductAndWriteReview()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);

            TestHelper.CaptureStepAndScreenshot(test, driver, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Write review", () => productsPage.WriteYourReview());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Submit review", () => productsPage.SubmitReview());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify success message", () => productsPage.CheckSuccessMessage());
        }

        [Test]
        public void SearchProductThenViewProductAndChangeQuantity()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);

            TestHelper.CaptureStepAndScreenshot(test, driver, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Set quantity", () => productsPage.SetQuantity(qualiti));
            TestHelper.CaptureStepAndScreenshot(test, driver, "Add to cart", () => productsPage.AddToCartInDetail());
            TestHelper.CaptureStepAndScreenshot(test, driver, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test, driver, "Verify product quantity in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
                cartPage.ProductQuantity(qualiti);
            });
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
    }

}
