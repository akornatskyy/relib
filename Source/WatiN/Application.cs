using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Supplemental.Collections;
using WatiN.Core;

namespace ReusableLibrary.WatiN
{
    public sealed class Application : Disposable
    {
        private readonly WebServer m_server;
        private readonly bool m_manageProcess;

        public static Application Attach<TBrowser>(string pathToWebSite, int webServerPort, bool manageProcess)
             where TBrowser : Browser
        {
            var homePage = new Uri(String.Format(CultureInfo.InvariantCulture, "http://localhost:{0}/", webServerPort));
            var app = new Application(WebServer.Attach(pathToWebSite, webServerPort, manageProcess),
                                      AttachBrowser<TBrowser>(homePage, manageProcess),
                                      homePage,
                                      manageProcess);
            return app;
        }

        public Uri HomePage { get; private set; }

        public Uri RelativeUri(string relativePath)
        {
            return new Uri(HomePage, new Uri(relativePath, UriKind.Relative));
        }

        public Browser Browser { get; private set; }

        private Application(WebServer server, Browser browser, Uri homePage, bool manageProcess)
        {
            m_server = server;
            Browser = browser;
            HomePage = homePage;
            m_manageProcess = manageProcess;
        }

        [DebuggerStepThrough]
        protected override void Dispose(bool disposing)
        {
            if (disposing && m_manageProcess)
            {
                m_server.Dispose();

                Browser.Close();
                Browser.Dispose();
                Browser = null;

                RecycleBrowserProcesses();
            }
        }

        private static TBrowser AttachBrowser<TBrowser>(Uri homePage, bool manageProcess) where TBrowser : Browser
        {
            if (manageProcess)
            {
                RecycleBrowserProcesses();
                Settings.WaitForCompleteTimeOut = 20;
            }

            TBrowser browser;
            if (Browser.Exists<TBrowser>(Find.Any))
            {
                browser = Browser.AttachTo<TBrowser>(Find.Any, 1);
            }
            else
            {
                browser = (TBrowser)Activator.CreateInstance(typeof(TBrowser));
            }

            try
            {
                browser.GoTo(homePage);
                browser.BringToFront();
                Thread.Sleep(100);
            }
            catch (global::WatiN.Core.Exceptions.TimeoutException tex)
            {
                Console.WriteLine(tex.Message);
                return AttachBrowser<TBrowser>(homePage, manageProcess);
            }

            return browser;
        }

        private static void RecycleBrowserProcesses()
        {
            RecycleProcesses(Process.GetProcessesByName("dwwin"));
            RecycleProcesses(Process.GetProcessesByName("vsjitdebugger"));
            RecycleProcesses(Process.GetProcessesByName("iexplore"));
        }

        private static void RecycleProcesses(IEnumerable<Process> processes)
        {
            processes.ForEach(p =>
            {
                ProcessHelper.Shutdown(p);
            });
        }
    }
}
