using System;
using EnvDTE;

namespace TcXae
{
    public class Solution : IDisposable
    {
        private const string ProgID = "TcXaeShell.DTE.15.0";
        private readonly Project _project;
        #pragma warning disable CA1416 // Plattformkompatibilität überprüfen
        #pragma warning disable CS8601 // Mögliche Nullverweiszuweisung.
        #pragma warning disable CS8600 // Das NULL-Literal oder ein möglicher NULL-Wert wird in einen Non-Nullable-Typ konvertiert.
        #pragma warning disable CS8604 // Mögliches Nullverweisargument.
        private readonly EnvDTE80.DTE2 _dte = (EnvDTE80.DTE2)Activator.CreateInstance(Type.GetTypeFromProgID(ProgID));
        #pragma warning restore CS8604 // Mögliches Nullverweisargument.
        #pragma warning restore CS8600 // Das NULL-Literal oder ein möglicher NULL-Wert wird in einen Non-Nullable-Typ konvertiert.
        #pragma warning restore CS8601 // Mögliche Nullverweiszuweisung.
        #pragma warning restore CA1416 // Plattformkompatibilität überprüfen

        private bool _disposed;

        #pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public Solution()
        #pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
            if (_dte != null)
            {
                _dte.SuppressUI = true;
                _dte.MainWindow.Visible = false;
                _dte.UserControl = false;
                _project = new Project(_dte.Solution);
            }
        }

        public Project Project
        {
            get { return _project; }
        }

        public bool Open(string solutionFilePath)
        {
            if (String.IsNullOrEmpty(solutionFilePath))
                { return false; }

            solutionFilePath = Path.GetFullPath(solutionFilePath).Replace("\\", "/");
            if (!System.IO.File.Exists(solutionFilePath))
                { return false; }

            MessageFilter.Register();
            try
            {
                _dte.Solution.Open(solutionFilePath);
                Task.Delay(1000).Wait();
            }
            catch (Exception)
            {
                Task.Delay(2000).Wait();
                _dte.Solution.Open(solutionFilePath);
            }

            if (_dte.Solution.IsDirty)
                { return false; }

            return _dte.Solution.IsOpen;
        }

        public bool Close()
        {
            _dte.Solution.Close();
            _dte.Quit();

            MessageFilter.Revoke();
            return true;
        }

        //Build Solution
        public bool Build()
        {
            _dte.Solution.SolutionBuild.Build(true);
            Task.Delay(500).Wait();
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