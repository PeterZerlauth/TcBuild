using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCatSysManagerLib;

namespace TcXae
{
    public class Project
    {
        private EnvDTE.Solution _solution;
        private ITcSysManager15? _systemManager;

        public Project (EnvDTE.Solution Solution) 
        { 
            this._solution = Solution;
        }

        public bool Open(string projectName)
        {
            if (String.IsNullOrEmpty(projectName) || (_solution == null))
            {
                return false;
            }
            try
            {
                foreach (EnvDTE.Project Project in _solution.Projects)
                {
                    if (Project.Name == projectName)
                    {
                        _systemManager = (ITcSysManager15)Project.Object;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public void Delete()
        {
            if (_solution != null)
            {
                foreach (EnvDTE.Project Project in _solution.Projects)
                {
                    Project.Delete();
                }
            }
        }

        public bool Save()
        {
            if (_solution != null)
            {
                foreach (EnvDTE.Project Project in _solution.Projects)
                {
                    Project.Save();
                }
            }
            return true;
        }

        public bool Save(string projectName, string newFileName)
        {
            if (_solution != null)
            {
                foreach (EnvDTE.Project Project in _solution.Projects)
                {
                    if (Project.Name == projectName)
                    {
                        Project.SaveAs(newFileName);
                        return Project.Saved;
                    }
                }
            }
            return false;
        }

        public bool Contains(string Name)
        {
            if (_systemManager != null)
            {
                try
                {
                    ITcSmTreeItem treeItem = _systemManager.LookupTreeItem($"TIPC^{Name}^{Name} Project");
                    ITcPlcIECProject2 iecProject = (ITcPlcIECProject2)treeItem;
                    if (iecProject != null)
                    {
                        return true;
                    }
                }
                catch { return false; }
            }
            return false;

        }




        public bool CheckAllObjects(string LibaryName)
        {
            if (!String.IsNullOrEmpty(LibaryName))
            {
                if (_systemManager != null)
                {
                    try
                    {
                        ITcSmTreeItem treeItem = _systemManager.LookupTreeItem($"TIPC^{LibaryName}^{LibaryName} Project");
                        ITcPlcIECProject2 iecProject = (ITcPlcIECProject2)treeItem;
                        if (iecProject != null)
                        {
                            return iecProject.CheckAllObjects();
                        }
                    }
                    catch (Exception)
                    {

                        return false;
                    }
                   
                    return false;
                }
                return false;
            }
            return false;
        }

        public bool BuildLibrary(string OutputPath, string LibaryName, bool Install)
        {
         
            string outputPath = Path.GetFullPath(OutputPath + "\\" + LibaryName + ".library").Replace("\\", "/");
          
            if (String.IsNullOrEmpty(OutputPath) || String.IsNullOrEmpty(LibaryName)) 
            {
                return false;
            }

            try
            {
                ITcSmTreeItem? treeItem = null;
                if (_systemManager != null)
                {
                   treeItem = _systemManager.LookupTreeItem($"TIPC^{LibaryName}^{LibaryName} Project");
                }
                
                ITcPlcIECProject2? iecProject = (ITcPlcIECProject2)treeItem;
                if (iecProject != null)
                {
                    iecProject.SaveAsLibrary(outputPath, Install);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool GenerateBootProject(string PlcName)
        {
            if(_systemManager!= null) 
            {
                Task.Delay(500).Wait();
                ITcSmTreeItem treeItem = _systemManager.LookupTreeItem($"TIPC^{PlcName}");
                ITcPlcProject iecProjectRoot = (ITcPlcProject)treeItem;
                iecProjectRoot.BootProjectAutostart = true;
                iecProjectRoot.GenerateBootProject(true);
                return true;
            }
            return false;
        }

        public bool StartRestartTwinCAT()
        {
            if (_systemManager!= null)
            {
                _systemManager.StartRestartTwinCAT();
                return true;
            }
            return false;
         
        }

        public bool IsTwinCATStarted()
        {
            Task.Delay(500).Wait();
            if (_systemManager != null)
            {
                return _systemManager.IsTwinCATStarted();
            }
            return false;
        }

            public bool ActivateConfiguration()
        {
            if (_systemManager != null)
            {
                _systemManager.ActivateConfiguration();
                return true;
            }
            return false;
        }

        public string NetId
        {
            get
            {
                if (_systemManager != null)
                {
                    return ((ITcSysManager2)_systemManager).GetTargetNetId();
                }
                return "";

            } 
            set
            {
                if (_systemManager != null)
                {
                    ((ITcSysManager2)_systemManager).SetTargetNetId(value);
                }
               
            }
        }




    }
}
