using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PTTKAutomatedTests
{
    public class Session
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string AppId = @"C:\Users\zmuda\Documents\Adam\Projektowanie Szpunar Projekt\ClientRepo\ProjektowanieOprogramowania\ConnecticoApplication\bin\Debug\ConnecticoApplication.exe";

        protected static WindowsDriver<WindowsElement> session;

        protected void Setup(string login, string password)
        {
            if (session == null)
            {
                AppiumOptions options = new AppiumOptions();
                options.AddAdditionalCapability("app", AppId);
                options.AddAdditionalCapability("deviceName", "WindowsPC");
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), options);
                Assert.IsNotNull(session);

                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);


                var usernameBox = session.FindElementByAccessibilityId("username_box");
                usernameBox.SendKeys(login);

                var paswordBox = session.FindElementByAccessibilityId("password_box");
                paswordBox.SendKeys(password);

                var loginButton = session.FindElementByAccessibilityId("loginButton");
                loginButton.Click();

                var loginWindowHandle = session.CurrentWindowHandle;
                int tries = 50;

                do //wait for main window
                {
                    Thread.Sleep(100);
                    tries--;
                } while (!session.WindowHandles.Any(h => h != loginWindowHandle) && tries > 0);

                Assert.IsTrue(session.WindowHandles.Any(h => h != loginWindowHandle));
                session.SwitchTo().Window(session.WindowHandles.First());
            }
        }

        protected void TearDown()
        {
            if (session != null)
            {
                CloseApp();
                session.Quit();
                session = null;
            }

            Thread.Sleep(100);

            foreach (var process in Process.GetProcessesByName("ConnecticoApplication"))
            {
                process.Kill();
            }
        }

        private static void CloseApp()
        {
            session.Close();
        }
    }
}
