using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;

namespace PTTKAutomatedTests
{
    [TestClass]
    public class AdminMountainGroups : Session
    {

        [TestInitialize]
        public void InitializeTest()
        {
            Setup("admin", "admin");
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TearDown();
        }

        [TestMethod]
        public void FilterListByName_ListChanged()
        {
            var panelButton = session.FindElementByAccessibilityId("mountainGroupsPanelButton");
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
            string groupName = cells[0].GetAttribute("Name");

            var filter = session.FindElementByAccessibilityId("NameTextBox");
            filter.SendKeys(groupName);

            rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.AreEqual(1, rows.Count);

            filter.Clear();
            rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.AreEqual(countBefore, rows.Count);
        }

        [TestMethod]
        public void FilterListByAbbreviation_ListChanged()
        {
            var panelButton = session.FindElementByAccessibilityId("mountainGroupsPanelButton");
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
            string abbreviationName = cells[1].GetAttribute("Name");

            var filter = session.FindElementByAccessibilityId("AbbreviationTextBox");
            filter.SendKeys(abbreviationName);

            rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.AreEqual(1, rows.Count);

            filter.Clear();
            rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.AreEqual(countBefore, rows.Count);
        }

        [TestMethod]
        public void EditMountainGroup_Success()
        {
            var panelButton = session.FindElementByAccessibilityId("mountainGroupsPanelButton");
            Assert.IsNotNull(panelButton);
            panelButton.Click();

            var dataGrid = session.FindElementByAccessibilityId("DataGrid");
            var rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.IsTrue(rows.Count > 1);

            var editButton = rows[0].FindElementByAccessibilityId("EditMountainGroupButton");
            editButton.Click();

            var names = rows.Select(r => r.FindElementsByClassName("DataGridCell")).Select(cs => cs[0].GetAttribute("Name")).ToList();

            var detailsWindow = session.FindElementByAccessibilityId("MountainGroupDetalis");
            var nameField = detailsWindow.FindElementByAccessibilityId("NameTextBox");
            var abbreviationField = detailsWindow.FindElementByAccessibilityId("AbbreviationTextBox");

            string newName;
            do
            {
                newName = GetRandomString(6);
            } while (names.Contains(newName));

            nameField.Clear();
            nameField.SendKeys(newName);

            var confirmButton = detailsWindow.FindElementByAccessibilityId("EnterDataButton");
            confirmButton.Click();

            var messageBoxWindow = session.FindElementByAccessibilityId("MessageBox");
            var title = messageBoxWindow.FindElementByAccessibilityId("TitleLabel").GetAttribute("Name");
            Assert.IsTrue(title.Contains("pomyślnie zapisane"));

            confirmButton = messageBoxWindow.FindElementByAccessibilityId("ConfirmButton");
            confirmButton.Click();

            try
            {
                session.FindElementByAccessibilityId("MountainGroupDetalis");
                Assert.Fail(); //window should not be visible
            }
            catch(WebDriverException) { }
        }

        [TestMethod]
        public void EditMountainGroup_Cancel_NoEdit()
        {
            var panelButton = session.FindElementByAccessibilityId("mountainGroupsPanelButton");
            Assert.IsNotNull(panelButton);
            panelButton.Click();

            var dataGrid = session.FindElementByAccessibilityId("DataGrid");
            var rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.IsTrue(rows.Count > 1);

            var editButton = rows[0].FindElementByAccessibilityId("EditMountainGroupButton");
            editButton.Click();

            var names = rows.Select(r => r.FindElementsByClassName("DataGridCell")).Select(cs => cs[0].GetAttribute("Name")).ToList();

            var detailsWindow = session.FindElementByAccessibilityId("MountainGroupDetalis");
            var nameField = detailsWindow.FindElementByAccessibilityId("NameTextBox");
            var abbreviationField = detailsWindow.FindElementByAccessibilityId("AbbreviationTextBox");

            string newName;
            do
            {
                newName = GetRandomString(6);
            } while (names.Contains(newName));

            nameField.Clear();
            nameField.SendKeys(newName);

            var exitButton = detailsWindow.FindElementByAccessibilityId("ExitButton");
            exitButton.Click();

            try
            {
                session.FindElementByAccessibilityId("MountainGroupDetalis");
                Assert.Fail(); //window should not be visible
            }
            catch (WebDriverException) { }

            var namesAfterCancelation = rows.Select(r => r.FindElementsByClassName("DataGridCell")).Select(cs => cs[0].GetAttribute("Name")).ToList();
            CollectionAssert.AreEqual(names, namesAfterCancelation);
        }

        [TestMethod]
        public void EditMountainGroup_Duplicate()
        {
            var panelButton = session.FindElementByAccessibilityId("mountainGroupsPanelButton");
            Assert.IsNotNull(panelButton);
            panelButton.Click();

            var dataGrid = session.FindElementByAccessibilityId("DataGrid");
            var rows = dataGrid.FindElementsByClassName("DataGridRow");
            Assert.IsTrue(rows.Count > 2);

            var editButton = rows[0].FindElementByAccessibilityId("EditMountainGroupButton");
            editButton.Click();

            var names = rows.Select(r => r.FindElementsByClassName("DataGridCell")).Select(cs => cs[0].GetAttribute("Name")).ToList();

            var detailsWindow = session.FindElementByAccessibilityId("MountainGroupDetalis");
            var nameField = detailsWindow.FindElementByAccessibilityId("NameTextBox");
            var abbreviationField = detailsWindow.FindElementByAccessibilityId("AbbreviationTextBox");

            string newName = names.Last();
            nameField.Clear();
            nameField.SendKeys(newName);

            var confirmButton = detailsWindow.FindElementByAccessibilityId("EnterDataButton");
            confirmButton.Click();

            var messageBoxWindow = session.FindElementByAccessibilityId("MessageBox");
            var title = messageBoxWindow.FindElementByAccessibilityId("TitleLabel").GetAttribute("Name");
            Assert.IsTrue(title.Contains("Duplikat"));

            confirmButton = messageBoxWindow.FindElementByAccessibilityId("ConfirmButton");
            confirmButton.Click();

            detailsWindow = session.FindElementByAccessibilityId("MountainGroupDetalis");
            var exitButton = detailsWindow.FindElementByAccessibilityId("ExitButton");
            exitButton.Click();
        }

        [TestMethod]
        public void OpenMountainsGroupsPnale_PanelVisible()
        {
            var panelButton = session.FindElementByAccessibilityId("mountainGroupsPanelButton");
            panelButton.Click();

            session.FindElementByAccessibilityId("NameTextBox"); //Filter on panel should be accessible
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
