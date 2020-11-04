using Infrastructure;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace Pages
{
    public class NavBar
    {
        private PageInteractions element;
        private IConfiguration config;

        private By homeElement = By.Id("homeNav");
        private By accountElement = By.Id("accountNav");
        private By loginElement = By.Id("loginNav");
        private By registerElement = By.Id("registerNav");
        private By projectElement = By.Id("projectNav");
        private By editAccountElement = By.Id("editAccountNav");
        private By logoutElement = By.Id("logoutNav");
        private By adminElement = By.Id("adminNav");

        public NavBar(IWebDriver driver, IConfiguration _config)
        {
            element = new PageInteractions(driver);
            config = _config;
        }
        public void NavigateToHomePage()
        {
            element.Click(homeElement);
        }

        public void NavigateToLoginPage()
        {
            element.Click(accountElement);
            element.WaitForElementToBeVisible(loginElement);
            element.Click(loginElement);
        }

        public void NavigateToRegisterPage()
        {
            element.Click(accountElement);
            element.WaitForElementToBeVisible(registerElement);
            element.Click(registerElement);
        }
    }
}
