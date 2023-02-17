using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestAPI
{
    public class UnitTest1
    {
        [Fact]
        public void TestEditStudent()
        {
            string url = "https://localhost:7183/new.html";
            //string url = "/html/body/div/table/tbody/tr[1]/td[6]/button[1]";

            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Until(wt => wt.FindElement(By.XPath(string.Format("/html/body/div/table/tbody/tr[2]/td[1]"))));
            IWebElement stdID = driver.FindElement(By.XPath(string.Format("/html/body/div/table/tbody/tr[2]/td[1]")));

            string id = stdID.Text;

            IWebElement editlink = driver.FindElement(By.XPath(string.Format("//*[@id=\"edit\"]")));

            editlink.Click();


            wait.Until(wt => wt.FindElement(By.XPath("/html/body/div/h1")));
            wait.Until(wt => wt.FindElement(By.XPath("/html/body/div/div/form/div[1]/table/tbody/tr/td")));

            IWebElement editID = driver.FindElement(By.XPath("/html/body/div/div/form/div[1]/table/tbody/tr/td"));

            IWebElement nameInput = driver.FindElement(By.Id("txtEdName"));
            nameInput.Clear();
            nameInput.SendKeys("Shiraz");

            IWebElement fatherNameInput = driver.FindElement(By.Id("txtEdFatherName"));
            fatherNameInput.Clear();
            fatherNameInput.SendKeys("Javed");

            IWebElement addressInput = driver.FindElement(By.Id("txtEdAddress"));
            addressInput.Clear();
            addressInput.SendKeys("Dubai");

            // Find the select element for courses and select a course
            WebDriverWait wait1 = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait1.Until(wt => wt.FindElement(By.XPath(string.Format("//option[contains(text(), 'Calculas')]"))));
            IWebElement courseSelect = driver.FindElement(By.Id("courseSelector"));
            SelectElement selectElement = new SelectElement(courseSelect);
            selectElement.SelectByText("Physics");
            
            IWebElement updateButton = driver.FindElement(By.Id("updatestd"));
            updateButton.Click();

            driver.Quit();

        }

        [Fact]
        public void CreateUnitTest()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://localhost:7183/new.html");


            // Find the input elements for student name, father name, and address, and enter the values
            IWebElement nameInput = driver.FindElement(By.Id("txtName"));
            nameInput.SendKeys("Shazaib");

            IWebElement fatherNameInput = driver.FindElement(By.Id("txtFatherName"));
            fatherNameInput.SendKeys("Javed");

            IWebElement addressInput = driver.FindElement(By.Id("txtAddress"));
            addressInput.SendKeys("Dubai");

            // Find the select element for courses and select a course
            IWebElement courseSelect = driver.FindElement(By.Id("courseSelector"));

            WebDriverWait wait1 = new WebDriverWait(driver, new TimeSpan(0, 0, 10));

            wait1.Until(wt => wt.FindElement(By.XPath(string.Format("//option[contains(text(), 'Calculas')]"))));
            IWebElement courseOption = courseSelect.FindElement(By.XPath("//option[contains(text(), 'Calculas')]"));
            courseOption.Click();

            // Submit the form to create the student
            IWebElement createButton = driver.FindElement(By.Id("createstd"));
            createButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Until(wt => wt.FindElement(By.CssSelector("#tblstd")));

            IWebElement refreshButton = driver.FindElement(By.Id("refresh"));
            refreshButton.Click();


            // Wait for the student to be added to the table

            IWebElement refreshButton2 = driver.FindElement(By.Id("refresh"));
            refreshButton2.Click();
            wait.Until(wt => wt.FindElement(By.XPath(string.Format("//tr[contains(td[2], 'Shazaib')]"))));
            IWebElement studentRow = driver.FindElement(By.XPath("//tr[contains(td[2], 'Shazaib')]"));

            driver.Quit();

        }


        [Fact]
        public void DeleteStudent()
        {
            string url = "https://localhost:7183/new.html";
            //string url = "/html/body/div/table/tbody/tr[1]/td[6]/button[1]";

            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Until(wt => wt.FindElement(By.XPath(string.Format("/html/body/div/table/tbody/tr[3]/td[1]"))));
            IWebElement stdID = driver.FindElement(By.XPath(string.Format("/html/body/div/table/tbody/tr[3]/td[1]")));

            IWebElement Deletelink = driver.FindElement(By.XPath(string.Format("//*[@id=\"tblstd\"]/tr[3]/td[6]/button[2]")));

            Deletelink.Click();
            driver.Quit();
        }



    }
}