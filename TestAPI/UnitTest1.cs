using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestAPI
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("2")]
        [InlineData("3")]
        public void TestEditStudent(string rownum)
        {
            string url = "https://localhost:7183/new.html";
            //string url = "/html/body/div/table/tbody/tr[1]/td[6]/button[1]";

            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Until(wt => wt.FindElement(By.XPath(string.Format("/html/body/div/table/tbody/tr[{0}]/td[1]", rownum))));
            IWebElement stdID = driver.FindElement(By.XPath(string.Format("/html/body/div/table/tbody/tr[{0}]/td[1]", rownum)));

            string id = stdID.Text;

            IWebElement editlink = driver.FindElement(By.XPath(string.Format("/html/body/div/table/tbody/tr[{0}]/td[6]/button[1]", rownum)));

            editlink.Click();

         
            wait.Until(wt => wt.FindElement(By.XPath("/html/body/div/h1")));
            wait.Until(wt => wt.FindElement(By.XPath("/html/body/div/div/form/div[1]/table/tbody/tr/td")));

            IWebElement editID = driver.FindElement(By.XPath("/html/body/div/div/form/div[1]/table/tbody/tr/td"));
            
            string eid = editID.Text;
            
            Assert.Equal(id, eid);

            driver.Close();

        }
    }
}