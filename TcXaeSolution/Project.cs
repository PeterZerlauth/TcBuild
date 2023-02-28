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
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(projectName))
                {
                    if (_solution != null)
                    {
                        foreach (EnvDTE.Project Project in _solution.Projects)
                        {
                            if (Project.Name == projectName)
                            {
                                _systemManager = (ITcSysManager15)Project.Object;

                                result = true;
                            }
                        }
                    }
                }
            }
            catch
            {
                result = false;
            }
           
            return result;
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
            if (!String.IsNullOrEmpty(OutputPath) && !String.IsNullOrEmpty(LibaryName))
            {
                string outputPath = Path.GetFullPath(OutputPath + "\\" + LibaryName + ".library").Replace("\\", "/");
                Directory.Exists(outputPath);
                if (_systemManager != null)
                {
                    try
                    {
                        ITcSmTreeItem treeItem = _systemManager.LookupTreeItem($"TIPC^{LibaryName}^{LibaryName} Project");
                        ITcPlcIECProject2 iecProject = (ITcPlcIECProject2)treeItem;
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
                return false;
            }
            return false;
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
