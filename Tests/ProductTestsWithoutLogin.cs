using BaiTapFPT.Helpers;
using BaiTapFPT.Pages;
using NUnit.Framework;

namespace BaiTapFPT.Tests
{
    [TestFixture("chrome")]
    [TestFixture("firefox")]
    [TestFixture("edge")]
    [Parallelizable(ParallelScope.All)]
    [Category("ProductWithOutLogin")]
    public class ProductTestsWithoutLogin : BaseTest
    {
        private ProductsPage productsPage;
        private CartPage cartPage;
        private string key = "Men";
        private int qualiti = 5;
        private readonly string browser;

        public ProductTestsWithoutLogin(string browser)
        {
            this.browser = browser;
        }

        //protected override string GetBrowser( ) => browser;
        [SetUp]
        public void SetUp()
        {
            productsPage = new ProductsPage(driver.Value);
            cartPage = new CartPage(driver.Value);
        }

        [Test]
        public void AddProduct()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void AddProductDouble()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click continue shopping", () => productsPage.ClickContinueShopping());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart again", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Check quantity in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
                cartPage.ProductQuantity(2);
            });
        }

        [Test]
        public void AddProductByCategory()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Select category 'MEN'", () => productsPage.SelectCategory());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify selected category", () =>
            {
                Assert.That(productsPage.GetCurrentCategory(), Is.EqualTo("Women > Dress"), "Category is incorrect");
            });
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void AddProductByBrand()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Select brand 'POLO'", () => productsPage.SelectBrand());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify selected brand", () =>
            {
                Assert.That(productsPage.GetActiveBrand(), Is.EqualTo("Polo"), "Brand is incorrect");
            });
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void AddProductWhenHover()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Hover over product", () => productsPage.HoverProduct());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click add to cart (hover)", () => productsPage.ClickAddToCartHover());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProduct()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);
        }

        [Test]
        public void SearchProductAndAddToCart()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Add product to cart", () => productsPage.ClickAddToCart());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify product in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
            });
        }

        [Test]
        public void SearchProductThenViewProductAndWriteReview()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Write review", () => productsPage.WriteYourReview());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Submit review", () => productsPage.SubmitReview());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify success message", () => productsPage.CheckSuccessMessage());
        }

        [Test]
        public void SearchProductThenViewProductAndChangeQuantity()
        {
            HideBottomAdBanner();
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open Product Page", () => productsPage.OpenProductPage());
            SearchProductWithKeyword(key);

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Click view product", () => productsPage.ClickViewProduct());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Set quantity", () => productsPage.SetQuantity(qualiti));
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Add to cart", () => productsPage.AddToCartInDetail());
            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Open cart", () => productsPage.ClickViewCart());

            TestHelper.CaptureStepAndScreenshot(test.Value, driver.Value, "Verify product quantity in cart", () =>
            {
                Assert.That(cartPage.IsProductInCart(), "Product doesn't exist in cart");
                cartPage.ProductQuantity(qualiti);
            });
        }

        private void SearchProductWithKeyword(string keyword)
        {
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
    }
}
