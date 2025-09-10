using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace MubintTests
{
    [TestFixture]
    public class MubintWebsiteTests
    {
        private IWebDriver? driver;
        private WebDriverWait? wait;

        [OneTimeSetUp]
        public void Setup()
        {
            // Установка совместимой версии ChromeDriver
            new WebDriverManager.DriverManager()
                .SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

            
            var options = new ChromeOptions();
            options.AddArguments("--no-sandbox", "--disable-dev-shm-usage", "--disable-gpu", "--window-size=1920,1080");

            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            driver.Url = "https://mubint.ru/";

            //  ожидание полной загрузки страницы
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        [Test]
        public void Test_01_PageTitleVerification()
        {
            // Проверка заголовка страницы
            string expectedTitle = "Поступление в ВУЗ, получение высшего образования очно и заочно";
            string actualTitle = driver!.Title;
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [Test]
        public void Test_02_VisibilityOfMainElements()
        {
            // Проверка видимости объектов
            IWebElement phone = wait!.Until(d => d.FindElement(By.ClassName("header-phone")));
            Assert.IsTrue(phone.Displayed);

            IWebElement menu = wait!.Until(d => d.FindElement(By.ClassName("top-menu")));
            Assert.IsTrue(menu.Displayed);

            IWebElement footer = wait!.Until(d => d.FindElement(By.ClassName("footer-top")));
            Assert.IsTrue(footer.Displayed);
        }

        [Test]
        public void Test_03_NavigationToAbiturientPage()
        {
            // Переход по ссылке
            IWebElement abiturientLink = wait!.Until(d => d.FindElement(By.CssSelector("a[href='/abitur/']")));
            abiturientLink.Click();

            wait!.Until(d => d.Url.Contains("abitur"));
            IWebElement pageTitle = wait!.Until(d => d.FindElement(By.ClassName("title")));
            Assert.IsTrue(pageTitle.Displayed);

            driver.Url = "https://mubint.ru/";
        }

        [Test]
        public void Test_04_ClickSearchButton()
        {
            // Эмуляция нажатия на кнопку поиска
            IWebElement searchButton = wait!.Until(d => d.FindElement(By.ClassName("search-btn")));
            searchButton.Click();

            // Проверка, что форма поиска появилась
            IWebElement searchForm = wait!.Until(d => d.FindElement(By.ClassName("search-form")));
            Assert.IsTrue(searchForm.Displayed);
        }

        [Test]
        public void Test_05_FillSearchField()
        {
            // Нажатие на кнопку поиска для отображения формы
            IWebElement searchButton = wait!.Until(d => d.FindElement(By.ClassName("search-btn")));
            searchButton.Click();

            // Заполнение текстового поля
            IWebElement searchField = wait!.Until(d => d.FindElement(By.Name("q")));
            searchField.Clear();
            searchField.SendKeys("образование");

            // Проверка, что текст введен правильно
            Assert.AreEqual("образование", searchField.GetAttribute("value"));

        }

        [OneTimeTearDown]
        public void FinalCleanup()
        {
            driver?.Quit();
        }
    }
}