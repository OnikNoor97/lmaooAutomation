using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using Pages;
using TechTalk.SpecFlow;

namespace TestRepository.Steps
{
    [Binding]
    public class NavBarSteps
    {
        private ScenarioContext context;
        private IWebDriver driver;
        private IConfiguration config;
        private NavBar navBar;

        public NavBarSteps(ScenarioContext injectedContext)
        {
            context = injectedContext;
            driver = context.Get<IWebDriver>("_driver");
            config = context.Get<IConfiguration>("config");
            navBar = new NavBar(driver, config);
        }

        [Given(@"I navigate to the Home Page")]
        public void GivenINavigateToTheHomePage()
        {
            navBar.NavigateToHomePage();
        }

        [Given(@"I navigate to the Login Page")]
        public void GivenINavigateToTheLoginPage()
        {
            navBar.NavigateToLoginPage();
        }


    }
}
