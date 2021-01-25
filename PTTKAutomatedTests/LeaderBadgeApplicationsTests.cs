using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PTTKAutomatedTests
{
    [TestClass]
    public class LeaderBadgeApplicationsTests : Session
    {
        [TestInitialize]
        public void InitializeTest()
        {
            Setup("przodownik2", "przodownik");
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TearDown();
        }

        [TestMethod]
        public void OpenBadgeApplicationPanel_PanelVisible()
        {
            var panelButton = session.FindElementByAccessibilityId("BageApplicationsPanelButton");
            panelButton.Click();

            session.FindElementByAccessibilityId("FirstnameTextBox"); //This should be accessible
        }

        [TestMethod]
        public void FilterListByFirstname_ListChanged()
        {
            var panelButton = session.FindElementByAccessibilityId("BageApplicationsPanelButton");
            Assert.IsNotNull(panelButton);
            panelButton.Click();

            var dataGrid = session.FindElementByAccessibilityId("DataGrid");

            ReadOnlyCollection<AppiumWebElement> rows;
            int tries = 5;
            do
            {
                Thread.Sleep(100);
                rows = dataGrid.FindElementsByClassName("DataGridRow");
                tries--;
            } while (rows.Count <= 0 && tries > 0);
            Assert.IsTrue(rows.Count > 1);

            int countBefore = rows.Count;

            var cells = rows[0].FindElementsByClassName("DataGridCell");
            Assert.IsTrue(cells.Count > 0);
            string name = cells[0].GetAttribute("Name");
            string firstname = name.Split(' ').First();

            var filter = session.FindElementByAccessibilityId("FirstnameTextBox");
            filter.SendKeys(firstname);

            rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.IsTrue(rows.Count < countBefore && rows.Count > 0);

            filter.Clear();
            rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.AreEqual(countBefore, rows.Count);
        }

        [TestMethod]
        public void FilterListByLastname_ListChanged()
        {
            var panelButton = session.FindElementByAccessibilityId("BageApplicationsPanelButton");
            Assert.IsNotNull(panelButton);
            panelButton.Click();

            var dataGrid = session.FindElementByAccessibilityId("DataGrid");

            ReadOnlyCollection<AppiumWebElement> rows;
            int tries = 5;
            do
            {
                Thread.Sleep(100);
                rows = dataGrid.FindElementsByClassName("DataGridRow");
                tries--;
            } while (rows.Count <= 0 && tries > 0);
            Assert.IsTrue(rows.Count > 1);

            int countBefore = rows.Count;

            var cells = rows[0].FindElementsByClassName("DataGridCell");
            Assert.IsTrue(cells.Count > 0);
            string name = cells[0].GetAttribute("Name");
            string firstname = name.Split(' ').Last();

            var filter = session.FindElementByAccessibilityId("LastnameTextBox");
            filter.SendKeys(firstname);

            rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.IsTrue(rows.Count < countBefore && rows.Count > 0);

            filter.Clear();
            rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.AreEqual(countBefore, rows.Count);
        }

        [TestMethod]
        public void EditApplicationDescription_Success()
        {
            var panelButton = session.FindElementByAccessibilityId("BageApplicationsPanelButton");
            Assert.IsNotNull(panelButton);
            panelButton.Click();

            var dataGrid = session.FindElementByAccessibilityId("DataGrid");
            var rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.IsTrue(rows.Count > 1);

            var assessButton = rows[0].FindElementByAccessibilityId("AssessApplicationButton");
            assessButton.Click();

            try
            {
                var messageBox = session.FindElementByAccessibilityId("MessageBox");
                var t = messageBox.FindElementByAccessibilityId("TitleLabel").GetAttribute("Name");
                Assert.IsTrue(t.Contains("mniejsza niż norma odznaki"));

                var confirm = messageBox.FindElementByAccessibilityId("ConfirmButton");
                confirm.Click();
            }
            catch (WebDriverException) { } //Warning will be displayed only for some 

            var detailsWindow = session.FindElementByAccessibilityId("BageApplicationDetails");
            var descriptionBox = detailsWindow.FindElementByAccessibilityId("DescriptionTextBox");

            string newDescription = GetRandomString(30);

            descriptionBox.Clear();
            descriptionBox.SendKeys(newDescription);

            var keppInProgressButton = detailsWindow.FindElementByAccessibilityId("KeepInProgressButton");
            keppInProgressButton.Click();

            var messageBoxWindow = session.FindElementByAccessibilityId("MessageBox");
            var title = messageBoxWindow.FindElementByAccessibilityId("TitleLabel").GetAttribute("Name");
            Assert.IsTrue(title.Contains("pomyślnie zapisane"));

            var confirmButton = messageBoxWindow.FindElementByAccessibilityId("ConfirmButton");
            confirmButton.Click();

            try
            {
                session.FindElementByAccessibilityId("BageApplicationDetails");
                Assert.Fail(); //window should not be visible
            }
            catch (WebDriverException) { }
        }

        [TestMethod]
        public void RejectApplicationWithoutDescription_Warning()
        {
            var panelButton = session.FindElementByAccessibilityId("BageApplicationsPanelButton");
            Assert.IsNotNull(panelButton);
            panelButton.Click();

            var dataGrid = session.FindElementByAccessibilityId("DataGrid");
            var rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.IsTrue(rows.Count > 1);

            var assessButton = rows[0].FindElementByAccessibilityId("AssessApplicationButton");
            assessButton.Click();

            try
            {
                var messageBox = session.FindElementByAccessibilityId("MessageBox");
                var t = messageBox.FindElementByAccessibilityId("TitleLabel").GetAttribute("Name");
                Assert.IsTrue(t.Contains("mniejsza niż norma odznaki"));

                var confirm = messageBox.FindElementByAccessibilityId("ConfirmButton");
                confirm.Click();
            }
            catch (WebDriverException) { } //Warning will be displayed only for some 

            var detailsWindow = session.FindElementByAccessibilityId("BageApplicationDetails");
            var descriptionBox = detailsWindow.FindElementByAccessibilityId("DescriptionTextBox");


            descriptionBox.Clear();

            var rejectButton = detailsWindow.FindElementByAccessibilityId("RejectButton");
            rejectButton.Click();

            var messageBoxWindow = session.FindElementByAccessibilityId("MessageBox");
            var title = messageBoxWindow.FindElementByAccessibilityId("TitleLabel").GetAttribute("Name");
            Assert.IsTrue(title.Contains("Brak opisu"));

            var confirmButton = messageBoxWindow.FindElementByAccessibilityId("ConfirmButton");
            confirmButton.Click();

            detailsWindow = session.FindElementByAccessibilityId("BageApplicationDetails");
            var exitButton = detailsWindow.FindElementByAccessibilityId("ExitButton");
            exitButton.Click();
        }

        private Random random = new Random();

        private string GetRandomString(int length)
        {
            return String.Concat(RandomSequence().Where(x => !char.IsControl(x)).Take(length));
        }

        private IEnumerable<char> RandomSequence()
        {
            while (true)
            {
                yield return (char)random.Next('a', 'z');
            }
        }
    }
}
