using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;

namespace Infrastructure
{
    public class PageInteractions
    {
        private IWebDriver _driver;
        private IJavaScriptExecutor js;
        public string Message;
        public string screenshotTitle;

        public PageInteractions(IWebDriver driver)
        {
            _driver = driver;
            js = (IJavaScriptExecutor)_driver;
        }

        public void FailMessage(By by, string function)
        {
            _driver.TakeScreenshot().SaveAsFile($"screens.png", ScreenshotImageFormat.Png);

            Message = $"Action attempted {function}: element located by {by.ToString()}";
            Assert.Fail($"Exception in {function}: element located by {by.ToString()} not visible and enabled within 10 seconds.");
        }

        public void SendKeys(By by, string value)
        {
            WaitForAjaxToFinish();
            WaitForPageDomLoadComplete();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            try
            {
                IWebElement myElement = wait.Until<IWebElement>(driver =>
                {
                    IWebElement element = _driver.FindElement(by);
                    return (element.Displayed && element.Enabled) ? element : null;
                }
                );
                js.ExecuteScript("arguments[0].scrollIntoView();", myElement);
                myElement.Click();
                myElement.Clear();
                myElement.SendKeys(value);
            }
            catch (WebDriverTimeoutException)
            {
                FailMessage(by, "SendKeys");
            }
        }
        public void WaitForPageDomLoadComplete()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            while (true) // Handle timeout somewhere
            {
                if (wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete")))
                {
                    break;
                }
            }
        }

        public void WaitForAjaxToFinish()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds((5)));
            while (true) // Handle timeout somewhere
            {
                if (wait.Until(driver => (bool)(_driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0")))
                {
                    break;
                }
            }
        }

        public void CloseDriver()
        {
            _driver.Close();
        }

        public void NavigateTo(string path)
        {
            string newPath = _driver.Url + path;
            _driver.Url = newPath;
        }

        public void NavigateToUrl(string url)
        {
            _driver.Url = url;
        }

        public void Refresh()
        {
            _driver.Navigate().Refresh();
        }

        public void SwitchToTab(int tabNumber)
        {
            _driver.SwitchTo().Window(_driver.WindowHandles[tabNumber]);
        }

        public void SwitchToFrame(string frame)
        {
            _driver.SwitchTo().Frame(frame);
        }

        public void SwitchToDefault()
        {
            _driver.SwitchTo().DefaultContent();
        }

        public void SwitchToAlert()
        {
            _driver.SwitchTo().Alert().Accept();
        }

        public String GetUrl()
        {
            return _driver.Url;
        }

        public void AddCookie(Cookie cookie)
        {
            _driver.Manage().Cookies.AddCookie(cookie);
        }

        public int GetWidth()
        {
            return _driver.Manage().Window.Size.Width;
        }

        public int WindowsHandleCount()
        {
            return _driver.WindowHandles.Count;
        }
    }
}