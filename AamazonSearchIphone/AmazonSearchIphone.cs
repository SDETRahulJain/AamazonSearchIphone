using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

namespace AmazonSearch
{
    [TestFixture]
    public class AmazonSearchTests
    {
       // private IWebDriver driver;
        private readonly string username = "technorahul09";
        private readonly string accessKey = "soQh2Ttzkc2kaqtjBchfWveR58nZNq9dSoGvab1wrIKjes8axt";
        private readonly string gridUrl = "hub.lambdatest.com/wd/hub";

        public static string LT_USERNAME = Environment.GetEnvironmentVariable("LT_USERNAME") == null ? "technorahul09" : Environment.GetEnvironmentVariable("LT_USERNAME");
        public static string LT_ACCESS_KEY = Environment.GetEnvironmentVariable("LT_ACCESS_KEY") == null ? "soQh2Ttzkc2kaqtjBchfWveR58nZNq9dSoGvab1wrIKjes8axt" : Environment.GetEnvironmentVariable("LT_ACCESS_KEY");
        public static bool tunnel = Boolean.Parse(Environment.GetEnvironmentVariable("LT_TUNNEL") == null ? "false" : Environment.GetEnvironmentVariable("LT_TUNNEL"));
        public static string build = Environment.GetEnvironmentVariable("LT_BUILD") == null ? "AamazonSearchIphone" : Environment.GetEnvironmentVariable("LT_BUILD");
        public static string seleniumUri = "https://hub.lambdatest.com:443/wd/hub";


        ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        private String browser;
        private String version;
        private String os;


        [SetUp]
        public void Setup()
        {
           // ChromeOptions options = new ChromeOptions();
           // options.BrowserVersion = "beta";
          //  options.AcceptInsecureCertificates = true;
            //  options.AddAdditionalOption("user", username);
            //   options.AddAdditionalOption("accessKey", accessKey);
            //  driver = new RemoteWebDriver(new Uri("https://" + username + ":" + accessKey + "@hub.lambdatest.com"), options);
            //  driver = new ChromeDriver(options);

            ChromeOptions capabilities = new ChromeOptions();
            capabilities.BrowserVersion = "beta";
            Dictionary<string, object> ltOptions = new Dictionary<string, object>();
            ltOptions.Add("username", "technorahul09");
            ltOptions.Add("accessKey", "soQh2Ttzkc2kaqtjBchfWveR58nZNq9dSoGvab1wrIKjes8axt");
            ltOptions.Add("platformName", "Windows 10");
            ltOptions.Add("project", "Untitled");
            ltOptions.Add("w3c", true);
            ltOptions.Add("console", "true");
            ltOptions.Add("project", "AmazonSearch");
            ltOptions.Add("build", "AmazonSearch");
            ltOptions.Add("plugin", "c#-c#");
            capabilities.AddAdditionalOption("LT:Options", ltOptions);

           
            driver.Value = new RemoteWebDriver(new Uri(seleniumUri), capabilities);
            Console.Out.WriteLine(driver);

        }

        [Test]
        public void SearchForIphone15AndGetPrices()
        {
            driver.Value.Navigate().GoToUrl("https://www.amazon.com");
            WebDriverWait wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(10));
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
            for (int i = 0;  i< phoneNames.Count; i++)
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
