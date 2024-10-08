﻿using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumAutomationLibrary.Utils
{
    public class AutomationInfrastructureHelper
    {
        private IWebDriver _driver;

        // Get ChromeDriver with an option for headless mode
        // Headless mode is useful for running tests in environments without a UI, such as CI/CD pipelines
        public IWebDriver GetDriver(bool headless = false)
        {
            var options = new ChromeOptions();
            if (headless)
            {
                options.AddArgument("--headless=new");  // Enables headless mode for Chrome
                options.AddArgument("window-size=1920,1080");  // Set window size to full HD resolution
                options.AddArgument("disable-gpu");  // Disable GPU rendering, necessary for headless mode
            }
            return new ChromeDriver(options);  // Returns a new ChromeDriver instance
        }

        // Switch to a new browser tab by simulating Ctrl + T keystrokes
        // This is useful for scenarios where a new tab is needed to open a link or perform an action
        public void SwitchToANewWindowKeyBased(IWebDriver _driver)
        {
            Actions action = new Actions(_driver);
            action.KeyDown(Keys.Control).SendKeys("t").KeyUp(Keys.Control).Perform();  // Simulate Ctrl + T for a new tab

            var windowHandles = _driver.WindowHandles;  // Get all window handles
            _driver.SwitchTo().Window(windowHandles.Last());  // Switch to the last (newly opened) window/tab
        }

        // Switch to a new window that is opened by clicking a button or link
        // Useful when the test triggers a new window popup, and the driver needs to handle it
        public void SwitchToNewWindow(IWebDriver _driver, By by)
        {
            string currentWindowHandle = _driver.CurrentWindowHandle;
            _driver.FindElement(by).Click();
            WebDriverWait webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10.0));
            webDriverWait.Until((IWebDriver d) => d.WindowHandles.Count > 1);
            ReadOnlyCollection<string> windowHandles = _driver.WindowHandles;
            foreach (string item in windowHandles)
            {
                if (item != currentWindowHandle)
                {
                    _driver.SwitchTo().Window(item);
                    break;
                }
            }
        }

        // Switch back to the original window
        // This method is useful when you need to switch back to the first window after performing actions on other windows
        public void SwitchToOriginalWindow(IWebDriver _driver)
        {
            
            var windowHandles = _driver.WindowHandles;
            // Close new window
            _driver.Close();
            // Switch to the first window
            _driver.SwitchTo().Window(windowHandles.First());
            
            
        }

        // Overloaded method to switch to the original window using its handle
        public void SwitchToOriginalWindow(string originalWindowHandle)
        {
            _driver.SwitchTo().Window(originalWindowHandle);  // Switch to the original window using its handle
        }

        // Scroll to a specific element on the page
        // Useful when the element is not in the visible part of the page and needs to be brought into view
        public void ScrollToElement(IWebDriver _driver, IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);  // Scroll the page until the element is in view
        }

        // Perform a mouse hover action on an element
        // Useful for revealing hidden elements, such as dropdowns, which appear on hovering over another element
        public void HoverOverElement(IWebDriver _driver, IWebElement element)
        {
            Actions actions = new Actions(_driver);
            actions.MoveToElement(element).Perform();  // Move the mouse pointer over the element
        }

        // Explicit wait: Wait for an element to be visible on the page
        // This is necessary for elements that load dynamically or take time to appear
        public void WaitForElementToBeVisible(IWebDriver driver, By locator, int timeoutInSeconds = 15)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(locator));  // Wait until the element is visible
        }

        // Explicit wait: Wait for an element to be clickable
        // Useful for elements that need to be clicked but aren't immediately ready (e.g., after AJAX requests)
        public void WaitForElementToBeClickable(IWebDriver driver, By locator, int timeoutInSeconds = 15)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.ElementToBeClickable(locator));  // Wait until the element is clickable
        }

        // Implicit wait: Set a default timeout for WebDriver to wait before throwing a NoSuchElementException
        // This is a global wait that applies to all element searches within the session
        public void SetImplicitWait(IWebDriver driver, int seconds = 15)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);  // Set implicit wait time
        }

        // Capture a screenshot of the current page
        // Saves the screenshot as a PNG file with a timestamp to avoid overwriting files
        public string CaptureScreenshot(IWebDriver _driver)
        {
            string screenshotPath = string.Empty;
            try
            {
                string screenshotsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                if (!Directory.Exists(screenshotsDirectory))
                {
                    Directory.CreateDirectory(screenshotsDirectory);
                }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");  // Generate a timestamp for the screenshot
                screenshotPath = Path.Combine(screenshotsDirectory, $"screenshot_{timestamp}.png"); //Save the screenshot with a timestamp in the filename

                Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                screenshot.SaveAsFile(screenshotPath);  // Save the screenshot to the specified path
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error capturing screenshot: {ex.Message}");  // Handle errors that may occur during the screenshot process
            }
            return screenshotPath;
        }
    }
}
