using System;
using EnvDTE;

namespace XAE
{
    public class Solution : IDisposable
    {
        private const string ProgID = "TcXaeShell.DTE.15.0";
        public Project Project;
        private EnvDTE80.DTE2 dte;
        private bool disposedValue;

        public Solution()
        {
            try
            {
                Type type = null;
                type = Type.GetTypeFromProgID(ProgID);
                dte = (EnvDTE80.DTE2)Activator.CreateInstance(type);
                dte.SuppressUI = true;
                dte.MainWindow.Visible = false;
                dte.UserControl = false;
                Project = new Project(dte.Solution);
            }
            catch
            {
                if (dte != null)
                {
                    dte.Quit();
                }
            }

        }

        public bool Open(string solutionFilePath)
        {
            if (!String.IsNullOrEmpty(solutionFilePath))
            {
                solutionFilePath = Path.GetFullPath(solutionFilePath).Replace("\\", "/");
                System.IO.File.Exists(solutionFilePath);
                MessageFilter.Register();
                dte.Solution.Open(solutionFilePath);
                System.Threading.Thread.Sleep(1000);
                MessageFilter.Revoke();
            }
            return dte.Solution.IsOpen;
        }

        public bool Close()
        {
            if (dte.Solution != null)
            {
                if (dte.Solution.IsOpen)
                {
                    dte.Solution.Close();
                }
            }
            if(dte != null)
            {
                dte.Quit();
            }
            MessageFilter.Revoke();
            return true;
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