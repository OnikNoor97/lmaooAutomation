namespace TestRepository.Hooks
{
    using BoDi;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using TechTalk.SpecFlow;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Edge;
    using OpenQA.Selenium.Support.Extensions;

    [Binding]
    public class Hooks
    {
        public static IConfigurationRoot Configuration;
        public static IObjectContainer _objectContainer;

        public static ScenarioContext _scenarioContext;
        public static FeatureContext _featureContext;
        public static IWebDriver driver;

        public static string TestRepoPath;
        public static string ScreenShotPath;


        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            TestRepoPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();
            Configuration = new ConfigurationBuilder().SetBasePath(TestRepoPath).AddJsonFile("appsettings.json").Build();


            ScreenShotPath = TestRepoPath + $"{Path.DirectorySeparatorChar}Screenshots{Path.DirectorySeparatorChar}";
            if (!Directory.Exists(ScreenShotPath))
            { 
                Directory.CreateDirectory(ScreenShotPath);
            }
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext feature)
        {
            _featureContext = feature;
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarios)
        {
            scenarios.Add("config", Configuration);
            IConfiguration config = scenarios.Get<IConfiguration>("config");

            string browserChoice = config["BrowserChoice"];
            _scenarioContext = scenarios;
            
            SelectBrowser(browserChoice);
            scenarios.Add("_driver", driver);
            driver.Manage().Window.Maximize();
        }

        internal void SelectBrowser(string browserChoice)
        {
            switch (browserChoice)
            {
                case "CHROME":
                    driver = new ChromeDriver();
                    break;

                case "CHROME-HEADLESS":
                    ChromeOptions option = new ChromeOptions();
                    option.AddArgument("--headless");
                    driver = new ChromeDriver(option);
                    break;

                case "FIREFOX":
                    driver = new FirefoxDriver();
                    break;

                case "IE":
                    driver = new InternetExplorerDriver();
                    break;

                case "EDGE":
                    driver = new EdgeDriver();
                    break;

                default:
                    throw new Exception("Web Driver not selected, in appsettings.json add BrowserChoice key");
            }
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenario)
        {
            string _reportname = $"{scenario.ScenarioInfo.Title}.png";

            if (File.Exists($"{TestRepoPath}{_reportname}")) File.Delete($"{TestRepoPath}{_reportname}");

            scenario.Get<IWebDriver>("_driver").TakeScreenshot().SaveAsFile($"{ScreenShotPath}{_reportname}", ScreenshotImageFormat.Png);
            scenario.Get<IWebDriver>("_driver").Quit();
        }
    }
}