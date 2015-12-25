using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Supplemental.Collections;

namespace ReusableLibrary.WatiN
{
    public sealed class WebServer : Disposable
    {
        private readonly Process m_serverProcess;
        private readonly bool m_manageProcess;

        [SecurityPermissionAttribute(SecurityAction.Demand, UnmanagedCode = true)]
        public static WebServer Attach(string pathToWebSite, int webServerPort, bool manageProcess)
        {
            Process serverProcess;
            var serverProcesses = Process.GetProcessesByName("WebDev.WebServer40");

            if (manageProcess)
            {
                serverProcesses.ForEach(p => ProcessHelper.Shutdown(p));
                serverProcess = CreateProcess(pathToWebSite, webServerPort);
            }
            else
            {
                if (serverProcesses.Length > 0)
                {
                    serverProcess = serverProcesses[0];
                }
                else
                {
                    serverProcess = CreateProcess(pathToWebSite, webServerPort);
                }
            }

            return new WebServer(serverProcess, manageProcess);
        }

        private WebServer(Process serverProcess, bool manageProcess)
        {
            m_serverProcess = serverProcess;
            m_manageProcess = manageProcess;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && m_manageProcess)
            {
                ProcessHelper.Shutdown(m_serverProcess);
            }
        }

        private static Process CreateProcess(string pathToWebSite, int webServerPort)
        {
            var serverProcess = new Process();
            serverProcess.StartInfo.FileName = Path.Combine(
                ProgramFilesX86(), 
                @"Common Files\Microsoft Shared\DevServer\10.0\WebDev.WebServer40.exe");
            serverProcess.StartInfo.Arguments = String.Format(
                CultureInfo.CurrentCulture,
                @"/port:{0} ""/path:{1}"" /vpath:{2}",
                webServerPort,
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathToWebSite)),
                "/");
            serverProcess.Start();
            return serverProcess;
        }

        private static string ProgramFilesX86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
    }
}
