using System;
using EnvDTE;
using EnvDTE80;
using TcRelease;

namespace TcRelease
{
    internal class Solution : IDisposable
    {

        public Project Project;
        private DTE2 dte;
        private bool disposedValue;

        public Solution() 
        {
            Type type = null;
            type = Type.GetTypeFromProgID("TcXaeShell.DTE.15.0");
            dte = (DTE2)System.Activator.CreateInstance(type);
            dte.SuppressUI = true;
            dte.MainWindow.Visible = false;
            dte.UserControl = false;
            Project = new Project(dte.Solution);
        }

        public void Open(string solutionFilePath)
        {
            solutionFilePath = Path.GetFullPath(solutionFilePath).Replace("\\", "/");
            System.IO.File.Exists(solutionFilePath);
            MessageFilter.Register();
            dte.Solution.Open(solutionFilePath);
            System.Threading.Thread.Sleep(1000);
            MessageFilter.Revoke();
        }

        public void Close()
        {
         

            if (dte.Solution != null)
            {
                if (dte.Solution.IsOpen) 
                {
                    dte.Solution.Close();
                }
            }
            dte.Quit();
            MessageFilter.Revoke();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
