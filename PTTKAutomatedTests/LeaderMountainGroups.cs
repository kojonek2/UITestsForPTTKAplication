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
    public class LeaderMountainGroups : Session
    {
        [TestInitialize]
        public void InitializeTest()
        {
            Setup("przodownik", "przodownik");
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TearDown();
        }

        [TestMethod]
        public void OpenMountainsGroupsPnale_PanelNotVisible()
        {
            var panelButton = session.FindElementByAccessibilityId("mountainGroupsPanelButton");
            panelButton.Click();

            try
            {
                session.FindElementByAccessibilityId("NameTextBox");
                Assert.Fail(); //should throw error because this element should not be visible 
            }
            catch (WebDriverException)
            {
            }
        }
    }
}
