using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTTKAutomatedTests
{
    [TestClass]
    public class TuristBadgeApplicationsTest : Session
    {
        [TestInitialize]
        public void InitializeTest()
        {
            Setup("turysta", "turysta123");
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TearDown();
        }

        [TestMethod]
        public void OpenBadgeApplicationPanel_PanelNotVisible()
        {
            var panelButton = session.FindElementByAccessibilityId("BageApplicationsPanelButton");
            panelButton.Click();

            try
            {
                session.FindElementByAccessibilityId("FirstnameTextBox");
                Assert.Fail(); //should throw error because this element should not be visible 
            }
            catch (WebDriverException)
            {
            }
        }
    }
}
