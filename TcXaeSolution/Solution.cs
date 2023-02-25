using System;
using EnvDTE;

namespace TcXae
{
    public class Solution : IDisposable
    {
        private const string ProgID = "TcXaeShell.DTE.15.0";
        private Project _project;
        private EnvDTE80.DTE2? _dte;
        private bool _disposed;

        public Solution()
        {
            try
            {
                #pragma warning disable CS8600 // Das NULL-Literal oder ein möglicher NULL-Wert wird in einen Non-Nullable-Typ konvertiert.
                #pragma warning disable CA1416 // Plattformkompatibilität überprüfen
                #pragma warning disable CS8604 // Mögliches Nullverweisargument.
                _dte = (EnvDTE80.DTE2)Activator.CreateInstance(Type.GetTypeFromProgID(ProgID));
                #pragma warning restore CS8604 // Mögliches Nullverweisargument.
                #pragma warning restore CA1416 // Plattformkompatibilität überprüfen
                #pragma warning restore CS8600 // Das NULL-Literal oder ein möglicher NULL-Wert wird in einen Non-Nullable-Typ konvertiert.
                if (_dte != null)
                {
                    _dte.SuppressUI = false;
                    _dte.MainWindow.Visible = true;
                    _dte.UserControl = true;
                    _project = new Project(_dte.Solution);
                }
            }
            catch
            {
                if (_dte != null)
                {
                    _dte.Quit();
                }
            }

        }

        public Project Project
        {
            get { return _project; }
        }

        public bool Open(string solutionFilePath)
        {
            try
            {
                if (!String.IsNullOrEmpty(solutionFilePath))
                {
                    solutionFilePath = Path.GetFullPath(solutionFilePath).Replace("\\", "/");
                    System.IO.File.Exists(solutionFilePath);
                    MessageFilter.Register();
                    if (_dte != null)
                    {
                        System.Threading.Thread.Sleep(1000);
                        _dte.Solution.Open(solutionFilePath);
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                if (_dte != null)
                {
                    return _dte.Solution.IsOpen;
                }
                else
                {
                    return false;
                }
                 
            }
            catch
            {
                _dte.Quit();
                return false;
            }
        }

        public bool Close()
        {
  
            if (_dte != null)
            {
                _dte.Solution.Close();
                _dte.Quit();
            }
            MessageFilter.Revoke();
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}