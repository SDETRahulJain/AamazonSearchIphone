using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

namespace AmazonSearch
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture("chrome", "beta")]
    [TestFixture("firefox", "89.0")]
    public class AmazonSearchTests
    {
        public static string seleniumUri = "https://hub.lambdatest.com:443/wd/hub";


        ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        private String browser;
        private String version;
        // private String os;
        public AmazonSearchTests(string browser, string version)
        {
            this.browser = browser;
            this.version = version;
        }

        [SetUp]
        public void Setup()
        {
            Dictionary<string, object> ltOptions = new Dictionary<string, object>();
            ltOptions.Add("username", "technorahul09");
            ltOptions.Add("accessKey", "soQh2Ttzkc2kaqtjBchfWveR58nZNq9dSoGvab1wrIKjes8axt");
            ltOptions.Add("platformName", "Windows 10");
            ltOptions.Add("w3c", true);
            ltOptions.Add("console", "true");
            ltOptions.Add("project", "AmazonSearch");
            ltOptions.Add("build", "AmazonSearch");
            ltOptions.Add("plugin", "c#-c#");
            if (browser.Equals("chrome", StringComparison.OrdinalIgnoreCase))
            {
                ChromeOptions capabilities = new ChromeOptions();
                capabilities.BrowserVersion = version;
                capabilities.AddAdditionalOption("LT:Options", ltOptions);
                driver.Value = new RemoteWebDriver(new Uri(seleniumUri), capabilities);
            }
            else if (browser.Equals("firefox", StringComparison.OrdinalIgnoreCase))
            {
                FirefoxOptions options = new FirefoxOptions();
                options.BrowserVersion = version;
                options.AddAdditionalOption("LT:Options", ltOptions);
                driver.Value = new RemoteWebDriver(new Uri(seleniumUri), options);
            }
            Console.Out.WriteLine(driver);

        }

        [Test]
        public void SearchForIphone15AndGetPrices()
        {
            driver.Value.Navigate().GoToUrl("https://www.amazon.com");
            WebDriverWait wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(30));
            IWebElement searchbox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("twotabsearchtextbox")));

            searchbox.SendKeys("iphone15" + Keys.Enter);
            //Wait for the search results to load
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            //Get the list of phones and their prices using JS
            var jsExecutor = (IJavaScriptExecutor)driver.Value;
            var phoneNames = (IList<object>)jsExecutor.ExecuteScript(
                "return [...document.querySelectorAll('div[data-component-type=\"s-search-result\"] h2 a')].map(e => e.textContent.trim());"
                );

            var phonePrices = (IList<object>)jsExecutor.ExecuteScript(
                "return [...document.querySelectorAll('div[data-component-type=\"s-search-result\"] span.a-price > span.a-offscreen')].map(e => e.textContent.trim());"
                );
            for (int i = 0; i < phoneNames.Count; i++)
            {
                Console.WriteLine(phoneNames[i] + " " + phonePrices[i]);
            }
        }

        [TearDown]
        public void Cleanup()
        {
            driver.Value.Quit();
        }
    }
}
