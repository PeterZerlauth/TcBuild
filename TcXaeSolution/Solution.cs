using System;
using EnvDTE;

namespace TcXae
{
    public class Solution : IDisposable
    {
        private const string ProgID = "TcXaeShell.DTE.15.0";
        private  Project _project;
        private  EnvDTE80.DTE2 _dte = (EnvDTE80.DTE2)Activator.CreateInstance(Type.GetTypeFromProgID(ProgID));
        private bool _disposed;

        public Solution()
        {
            _dte.SuppressUI = false;
            _dte.MainWindow.Visible = true;
            _dte.UserControl = true;
            _project = new Project(_dte.Solution);
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
                Task.Delay(1000).Wait();
                _dte.Solution.Open(solutionFilePath);
                Task.Delay(1000).Wait();
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
        //public void Build(bool save)
        //{
        //}


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